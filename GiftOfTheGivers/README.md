# Gift of the Givers — Sprint 1 Prototype

ASP.NET Core 8 MVC prototype for the Gift of the Givers disaster relief application,
matching the schema and user stories in the Part 1 planning report.

## What's implemented (Sprint 1 scope)

- Branding, navigation bar, responsive Bootstrap layout
- ASP.NET Core Identity authentication with **Donor**, **Volunteer**, and **Employee** roles
- Employee dashboard: create/edit relief projects, view registered volunteers
- Donor flow: browse active projects, donate (logged in **or as a guest**), choose
  currency (ZAR/USD/EUR) and one-time/recurring, placeholder on-screen tax certificate
- Volunteer flow: registration form (skills, availability, city) stored via EF Core
- Database via EF Core / Azure SQL, matching the ERD in Section B of the report
  (Users, Volunteers, ReliefProjects, VolunteerAssignments, Donations)

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB is fine for local dev) or an Azure SQL database
- Visual Studio 2022 / VS Code / Rider

## Run locally

```bash
cd GiftOfTheGivers
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

The app seeds the Donor/Volunteer/Employee roles, a demo Employee account
(`employee@giftofthegivers.local` / `Employee@123` — **change this before any
public deployment**), and two sample relief projects on first run.

## Deploy to Azure App Services

1. **Create an Azure SQL Database** (Azure Portal → SQL databases → Create).
   Note the server name, database name, admin login, and password.
2. **Update the connection string** — in `appsettings.json` for local testing,
   or (recommended) as an App Service **Connection String** app setting named
   `DefaultConnection` of type `SQLAzure`, so the real credentials never sit in
   source control.
3. **Create the App Service**: Azure Portal → App Services → Create →
   Runtime stack: **.NET 8 (LTS)** → Linux (cheaper) or Windows.
4. **Publish**:
   - Visual Studio: right-click the project → Publish → Azure → Azure App Service.
   - Or CLI:
     ```bash
     dotnet publish -c Release -o ./publish
     az webapp up --name <your-app-name> --resource-group <your-rg> --runtime "DOTNETCORE:8.0"
     ```
5. On first request, `Program.cs` calls `db.Database.Migrate()`, so the schema
   is created automatically against the Azure SQL database — no manual script
   needed, as long as the connection string app setting is correct.
6. Take screenshots at each step above (App Service creation, connection
   string configuration, Deployment Center / publish output, and the live app)
   for the deployment evidence required in Section 3.3 of the report.

## Notes for the team

- Tax certificate generation is currently a placeholder view (`Donation/Certificate.cshtml`) —
  swapping it for a real PDF is scoped for Sprint 2.
- Volunteer-to-project assignment (the `VolunteerAssignments` table) has a model
  and migration but no UI yet — also scoped for Sprint 2, per the sprint plan
  in Section 1.3 of the report.
