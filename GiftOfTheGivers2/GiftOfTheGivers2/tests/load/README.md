# Apache JMeter load test

This plan targets the local ASP.NET Core application only. It uses HTTP requests (not a payment gateway) to exercise the current anonymous donation and volunteer-registration web forms. It retrieves each ASP.NET anti-forgery token and retains cookies per virtual user.

## Test profile

- 20 virtual users by default (`-Jusers=20`)
- 20-second ramp-up (`-JrampSeconds=20`)
- 3 iterations per user (60 donation submissions and 60 volunteer registrations in total)
- Anonymous recurring donation in USD, followed by volunteer registration
- Response assertions on both confirmation pages

## Run

1. Start the web app on the local HTTP profile (`http://localhost:5082`).
2. If this is a repeat run, delete the previous `artifacts/jmeter/dashboard` folder first (JMeter requires the report output directory to be empty).
3. Install Apache JMeter 5.6.3 or later and ensure `jmeter` is on PATH.
4. From the repository root, run in PowerShell:

```powershell
New-Item -ItemType Directory -Force .\artifacts\jmeter | Out-Null
jmeter -n -t .\tests\load\GiftOfTheGivers_LoadTest.jmx -Jhost=localhost -Jport=5082 -Jprotocol=http -Jusers=20 -JrampSeconds=20 -JresultsFile=artifacts/jmeter/results.jtl -l .\artifacts\jmeter\results.jtl -e -o .\artifacts\jmeter\dashboard
```

For another local test host, override `-Jhost`, `-Jport` and `-Jprotocol`. Do not use the live demo site or production database. Start with 20 users; only increase after checking local machine capacity and getting team approval.

## Evidence to capture after a real run

- JMeter Summary Report and Aggregate Report
- HTML dashboard: response-time percentiles, throughput, response codes and active threads
- `results.jtl` (keep it with the submission evidence)
- A screenshot showing the 20-user Thread Group settings

No performance values are pre-filled: the numbers and graphs must come from an actual run on the team's test environment.
