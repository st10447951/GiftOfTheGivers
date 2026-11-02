using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace GiftOfTheGivers2.UITests;

internal static class BrowserDriverFactory
{
    public static IWebDriver Create(string browser)
    {
        bool headless = !string.Equals(
            Environment.GetEnvironmentVariable("GOTG_HEADLESS"), "false",
            StringComparison.OrdinalIgnoreCase);

        IWebDriver driver = browser.ToLowerInvariant() switch
        {
            "chrome" => CreateChrome(headless),
            "edge" => CreateEdge(headless),
            _ => throw new ArgumentException($"Unsupported browser '{browser}'. Use chrome or edge.")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        driver.Manage().Window.Size = new System.Drawing.Size(1440, 1000);
        return driver;
    }

    private static IWebDriver CreateChrome(bool headless)
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1440,1000");
        options.AddArgument("--disable-notifications");
        if (headless) options.AddArgument("--headless=new");
        return new ChromeDriver(options); // Selenium Manager resolves the matching driver.
    }

    private static IWebDriver CreateEdge(bool headless)
    {
        var options = new EdgeOptions();
        options.AddArgument("--window-size=1440,1000");
        options.AddArgument("--disable-notifications");

        if (headless)
            options.AddArgument("--headless=new");

        var service = EdgeDriverService.CreateDefaultService(
            @"C:\WebDrivers\Edge");

        return new EdgeDriver(service, options);
    }
}
