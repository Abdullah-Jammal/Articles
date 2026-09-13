# Regression checks

Run from the directory containing `Articles.slnx` with the .NET 10 SDK:

```powershell
dotnet build Articles.slnx
dotnet run --project Tests/Articles.RegressionTests -c Release
```

This executable test harness exits with an error when an assertion fails. It checks collection helpers, user/profile creation, profile field assignment, Entity Framework model validation and column names, confirmation email addresses and HTML, JWT signatures and claims, and the API's JWT authentication configuration. It requires no live database, SMTP server, or extra test framework packages.

The profile is owned by `User` and mapped to the existing profile columns in `AspNetUsers`. The context now uses the custom `UserRole` type, and refresh tokens have an explicit key. No database migrations are applied by these checks; an existing database's schema must be compared with this model before deployment.

Container builds use the directory containing `Articles.slnx` as their build context, for example:

```powershell
docker build -f Services/Auth/Auth.API/Dockerfile -t articles-auth .
```

The checks do not verify live database operations, SMTP delivery, Redis/MongoDB connectivity, or unfinished application features.
