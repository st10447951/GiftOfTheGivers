
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace GiftOfTheGivers2.UITests;

public class GiftOfTheGiversBrowserTests
{
    private const string BaseUrl = "https://localhost:7254";

    public static IEnumerable<object[]> Browsers => new[]
    {
        new object[] { "chrome" },
        new object[] { "edge" }
    };

    // 1. Valid employee login
    [Theory]
    [MemberData(nameof(Browsers))]
    public void Employee_can_log_in_with_valid_credentials(string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);
        var email = "employee@giftofthegivers.org";
        var password = "Employee@123";

        driver.Navigate().GoToUrl(BaseUrl + "/Identity/Accounts/Login");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
            ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState")
                ?.ToString() == "complete");

        WaitFor(driver, By.Name("Input.Email")).SendKeys(email);
        driver.FindElement(By.Name("Input.Password")).SendKeys(password);
        driver.FindElement(By.Id("login-submit")).Click();

        try
        {
            wait.Until(d =>
                d.FindElements(By.Id("logout")).Any());
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("LOGIN DIAGNOSTICS");
            Console.WriteLine("Browser: " + browser);
            Console.WriteLine("Current URL: " + driver.Url);
            Console.WriteLine("Page title: " + driver.Title);
            Console.WriteLine("Page content: " + driver.FindElement(By.TagName("body")).Text);

            throw new Exception(
                "Login did not succeed. Check the page content above for the login error.");
        }

        driver.Navigate().GoToUrl(BaseUrl + "/Employee/Dashboard");

        Assert.Contains(
            "Employee Dashboard",
            WaitForText(driver, "Employee Dashboard"));

        SaveScreenshot(driver, browser, "employee-login-valid");
    }
    // 2. Invalid employee password

    [Theory]
    [MemberData(nameof(Browsers))]
    public void Employee_login_rejects_invalid_password(string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);

        var email = Environment.GetEnvironmentVariable(
            "GOTG_TEST_EMPLOYEE_EMAIL")
            ?? "employee@giftofthegivers.org";

        driver.Navigate().GoToUrl(BaseUrl + "/Identity/Accounts/Login");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        wait.Until(d =>
            ((IJavaScriptExecutor)d)
                .ExecuteScript("return document.readyState")
                ?.ToString() == "complete");

        // Capture what the automated browser actually receives within.
        var pageSource = driver.PageSource;

        Console.WriteLine("========== LOGIN DIAGNOSTICS ==========");
        Console.WriteLine("BROWSER: " + browser);
        Console.WriteLine("URL: " + driver.Url);
        Console.WriteLine("TITLE: " + driver.Title);
        Console.WriteLine("EMAIL ID COUNT: " +
            driver.FindElements(By.Id("Input_Email")).Count);
        Console.WriteLine("EMAIL NAME COUNT: " +
            driver.FindElements(By.Name("Input.Email")).Count);
        Console.WriteLine("EMAIL TYPE COUNT: " +
            driver.FindElements(By.CssSelector("input[type='email']")).Count);
        Console.WriteLine("PAGE SOURCE:");
        Console.WriteLine(pageSource);
        Console.WriteLine("========== END DIAGNOSTICS ==========");

        // Check the field before attempting to type.
        var emailFields = driver.FindElements(By.Id("Input_Email"));

        Assert.True(
            emailFields.Count > 0,
            "Selenium did not receive an email input. Check the LOGIN DIAGNOSTICS in Test Explorer output.");

        emailFields[0].SendKeys(email);

        var passwordFields =
            driver.FindElements(By.Id("Input_Password"));

        Assert.True(
            passwordFields.Count > 0,
            "Selenium did not receive the password input.");

        passwordFields[0].SendKeys(
            "Definitely-Wrong-Password-123!");

        driver.FindElement(By.Id("login-submit")).Click();

        var page = WaitForText(driver, "Invalid login attempt.");

        Assert.Contains("Invalid login attempt.", page);

        SaveScreenshot(driver, browser, "employee-login-invalid");
    }


    // 3. Anonymous donation
    [Theory]
    [MemberData(nameof(Browsers))]
    public void Anonymous_donation_can_be_submitted(string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);

        driver.Navigate().GoToUrl(BaseUrl + "/Donations");

        var amount = WaitFor(driver, By.Id("Amount"));
        amount.Clear();
        amount.SendKeys("25.00");

        var anonymous = driver.FindElement(By.Id("IsAnonymous"));

        if (!anonymous.Selected)
            anonymous.Click();

        new SelectElement(driver.FindElement(By.Id("Currency")))
            .SelectByValue("ZAR");

        driver.FindElement(
            By.CssSelector("form button[type='submit']")
        ).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

        wait.Until(d =>
            d.Url.Contains("/Donations/Confirmation",
                StringComparison.OrdinalIgnoreCase)
            && d.PageSource.Contains(
                "Thank You for Your Donation",
                StringComparison.OrdinalIgnoreCase));

        Assert.Contains("Anonymous Donor", driver.PageSource);

        SaveScreenshot(driver, browser, "anonymous-donation-success");
    }

    // 4. Recurring USD donation
    [Theory]
    [MemberData(nameof(Browsers))]
    public void Recurring_USD_donation_can_be_submitted(string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);

        driver.Navigate().GoToUrl(BaseUrl + "/Donations");

        WaitFor(driver, By.Id("DonorName"))
            .SendKeys("UI Test Donor");

        var amount = WaitFor(driver, By.Id("Amount"));
        amount.Clear();
        amount.SendKeys("12.50");

        new SelectElement(driver.FindElement(By.Id("Currency")))
            .SelectByValue("USD");

        new SelectElement(driver.FindElement(By.Id("DonationType")))
            .SelectByValue("Recurring");

        driver.FindElement(
            By.CssSelector("form button[type='submit']")
        ).Click();

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

        wait.Until(d =>
            d.Url.Contains("/Donations/Confirmation",
                StringComparison.OrdinalIgnoreCase));

        var page = driver.FindElement(By.TagName("body")).Text;

        Assert.Contains("Thank You for Your Donation", page);
        Assert.True(
            page.Contains("12.50") || page.Contains("12,50"),
            "Donation amount was not displayed. Actual page:\n" + page);

        Assert.Contains("USD", page);
        Assert.Contains("Recurring", page);

        SaveScreenshot(driver, browser, "recurring-usd-donation-success");
    }

    // 5. Volunteer registration
    [Theory]
    [MemberData(nameof(Browsers))]
    public void Volunteer_registration_can_be_submitted(string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);

        driver.Navigate().GoToUrl(BaseUrl + "/Volunteers/Register");

        WaitFor(driver, By.Id("FullName"))
            .SendKeys("Tshego Selenium Test");

        driver.FindElement(By.Id("Email"))
            .SendKeys($"tshego.selenium.{Guid.NewGuid():N}@example.test");

        driver.FindElement(By.Id("Skills"))
            .SendKeys("First Aid, Logistics");

        new SelectElement(driver.FindElement(By.Id("Availability")))
            .SelectByValue("Weekends");

        driver.FindElement(
            By.CssSelector("form button[type='submit']")
        ).Click();

        Assert.Contains("Thank You for Registering",
            WaitForText(driver, "Thank You for Registering"));

        SaveScreenshot(driver, browser, "volunteer-registration-success");
    }

    // 6. Invalid volunteer form
    [Theory]
    [MemberData(nameof(Browsers))]
    public void Invalid_volunteer_form_displays_server_validation_errors(
        string browser)
    {
        using var driver = BrowserDriverFactory.Create(browser);

        driver.Navigate().GoToUrl(BaseUrl + "/Volunteers/Register");

        var form = WaitFor(driver, By.CssSelector("form"));

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "document.querySelectorAll('input,select').forEach(e => e.removeAttribute('required'));" +
            "document.querySelectorAll('input').forEach(e => {" +
            "if(e.type !== 'hidden') e.value = ''; });");

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].submit();", form);

        var page = WaitForText(driver, "Please enter your full name.");

        Assert.Contains("Please enter a contact email.", page);
        Assert.Contains("Please list at least one skill.", page);

        SaveScreenshot(driver, browser, "volunteer-invalid-form");
    }

    // Shared Selenium helpers
    private static IWebElement WaitFor(IWebDriver driver, By selector)
    {
        return new WebDriverWait(driver, TimeSpan.FromSeconds(15))
            .Until(d => d.FindElement(selector));
    }

    private static string WaitForText(IWebDriver driver, string expected)
    {
        return new WebDriverWait(driver, TimeSpan.FromSeconds(15))
            .Until(d =>
                d.PageSource.Contains(expected, StringComparison.Ordinal)
                    ? d.PageSource
                    : null)!;
    }

    private static string RequiredSetting(string name)
    {
        var value = Environment.GetEnvironmentVariable(name);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Set the {name} environment variable before running this test. " +
                "Use a local test account only. Do not use a production password.");
        }

        return value;
    }

    private static void SaveScreenshot(
        IWebDriver driver,
        string browser,
        string testName)
    {
        var directory = Path.GetFullPath(
            Path.Combine(
                AppContext.BaseDirectory,
                "..", "..", "..", "..", "..",
                "artifacts", "selenium", "screenshots"));

        Directory.CreateDirectory(directory);

        var file = Path.Combine(
            directory,
            $"{browser}-{testName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");

        ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(file);
    }
}
//closes.