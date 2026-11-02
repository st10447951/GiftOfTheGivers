# Gift of the Givers – Selenium UI tests (C#)

This test project stays in **C#** and targets **.NET 10**, matching the web application. It uses xUnit and Selenium WebDriver. Selenium Manager (included with Selenium 4) resolves the matching Chrome/Edge driver; install the browser itself first.

## Prerequisites

- .NET 10 SDK
- Google Chrome and Microsoft Edge installed
- The GiftOfTheGivers2 web app running locally or on a dedicated test copy
- A local Employee test account with the Employee role

## Start the application

From the repository root:

```powershell
dotnet run --project .\GiftOfTheGivers2.csproj --launch-profile http
```

The supplied HTTP launch profile uses `http://localhost:5082`. Do not point tests or load tests at the public/live demo.

## Configure employee test credentials

Set these environment variables in the same terminal that runs the tests. Use a disposable local test account, never a production account:

```powershell
$env:GOTG_BASE_URL = "http://localhost:5082"
$env:GOTG_TEST_EMPLOYEE_EMAIL = "your-local-employee-test-email"
$env:GOTG_TEST_EMPLOYEE_PASSWORD = "your-local-test-password"
$env:GOTG_HEADLESS = "true"
```

The valid-login test requires the environment variables. The invalid-login test uses the configured email if supplied, otherwise the locally seeded employee email. The current application seed account is defined in `Data/SeedData.cs`; review that file and keep it local/test-only.

## Run the tests

From the repository root:

```powershell
dotnet test .\tests\GiftOfTheGivers2.UITests\GiftOfTheGivers2.UITests.csproj --logger "console;verbosity=normal"
```

Each scenario runs in Chrome and Edge (12 browser/scenario combinations). Selenium Manager may download a compatible driver on first run; allow network access for that first run. Set `GOTG_HEADLESS=false` to watch the browsers.

The suite covers valid/invalid employee login, anonymous ZAR donation, recurring USD donation, successful volunteer registration, and server-side validation of an invalid volunteer form. Screenshots are written to `artifacts/selenium/screenshots/` when the tests execute. The screenshot files are real execution evidence; this repository does not include fabricated test screenshots.

## Important scope note

The current application records donations in an in-memory store. These UI tests verify the current form and confirmation workflow only; they do not prove that a recurring payment is charged on a schedule or that a payment gateway is integrated.
