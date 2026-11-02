# Tshego – UI automation and performance testing evidence

## Scope

The automated UI suite is written in C# with xUnit and Selenium WebDriver, matching the application's C# / ASP.NET Core MVC technology. It covers employee login, donations, volunteer registration and validation in Chrome and Edge. The JMeter plan targets the local/test copy only.

## Scenario coverage

| ID | Scenario | Expected outcome | Automation |
|---|---|---|---|
| UI-01 | Employee login with valid test account | Employee dashboard is displayed | Selenium / Chrome + Edge |
| UI-02 | Employee login with incorrect password | Login is rejected with an error | Selenium / Chrome + Edge |
| UI-03 | Anonymous donation | Confirmation identifies anonymous donor | Selenium / Chrome + Edge |
| UI-04 | Recurring donation in USD | Confirmation shows USD and recurring type | Selenium / Chrome + Edge |
| UI-05 | Volunteer registration | Registration confirmation is displayed | Selenium / Chrome + Edge |
| UI-06 | Invalid volunteer form | Server-side validation messages are displayed | Selenium / Chrome + Edge |
| LOAD-01 | 20 virtual users submit donations and volunteer registrations | Confirmation responses succeed; response metrics are recorded | Apache JMeter |

## Execution record – complete after running

| Item | Actual result | Evidence file / screenshot |
|---|---|---|
| Selenium execution date/time | To be recorded | `artifacts/selenium/screenshots/` |
| Chrome passed / failed | To be recorded | Test Explorer screenshot |
| Edge passed / failed | To be recorded | Test Explorer screenshot |
| JMeter virtual users / ramp-up / loops | Default plan: 20 / 20 seconds / 3 | JMeter Thread Group screenshot |
| Average response time | To be measured | JMeter dashboard |
| Median response time | To be measured | JMeter dashboard |
| 90th / 95th percentile response time | To be measured | JMeter dashboard |
| Throughput (requests/second) | To be measured | JMeter dashboard |
| Error percentage | To be measured | JMeter dashboard |

**Do not submit this report with “To be recorded” fields.** Replace them with actual measurements from the team's local/test environment and attach the matching screenshots and `.jtl` file.

## Bottleneck analysis template

After the load test, inspect the JMeter dashboard and application logs. Record only bottlenecks supported by the measurements. Consider: (1) slow server-side request processing; (2) database or in-memory-store contention; (3) CPU or memory saturation on the test host; (4) repeated redirects or expensive page rendering; and (5) validation or anti-forgery failures.

| Observation | Evidence (metric/log/time window) | Likely cause | Recommended action |
|---|---|---|---|
| To be completed after test | To be completed | To be confirmed | To be agreed with team |

## Limitations to disclose

- The supplied app currently uses in-memory donation and volunteer stores. Data resets when the app restarts and results are not a database performance benchmark.
- The recurring donation scenario currently verifies the form selection and confirmation only. It does not prove that a payment provider charges a recurring payment.
- The test plan measures web form endpoints, not external payment providers or Azure SQL.
- Performance results depend on the local test machine and environment; do not present them as production capacity.

## Phase 7 screenshot checklist

- [ ] Annotated screenshot of valid employee dashboard after login (hide email if required by the team's privacy rules).
- [ ] Invalid login error.
- [ ] Anonymous donation confirmation.
- [ ] Recurring USD donation confirmation.
- [ ] Volunteer registration confirmation.
- [ ] Invalid form validation messages.
- [ ] Chrome and Edge test results.
- [ ] JMeter Thread Group showing 20+ users.
- [ ] JMeter dashboard graphs and summary.
- [ ] Application logs around any errors or slow requests.

## Individual reflection – draft to personalize after execution

I implemented browser-based tests for the main employee, donor and volunteer journeys and prepared a repeatable JMeter workload for the local test application. The testing helped me check both successful and invalid submissions across two browsers; I will finalize this reflection with the actual results, issues found and improvements made during execution.
