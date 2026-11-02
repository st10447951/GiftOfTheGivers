# Connection string security handover

The uploaded source archive contained a SQL connection string with a password in `appsettings.json`. The copy prepared for Tshego's handover has that value redacted. Treat the original value as compromised: the repository owner should rotate the SQL password, remove the exposed value from Git history where feasible, and store the replacement in .NET User Secrets for local development and Azure App Service / pipeline secret settings for deployment.

The current `Program.cs` configures EF Core InMemory, so the placeholder connection-string value is not used to connect to SQL. Before switching to `UseSqlServer`, set a real connection string securely, for example:

```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your-new-connection-string>"
```

Do not commit the replacement secret. This security handover is included because the uploaded archive contained a credential; Lindokuhle / the repository owner should complete and verify rotation.
