using Article.Security;
using Articles.Abstractions.Enums;
using Auth.API.Features.Users.CreateUser;
using Auth.Application;
using Auth.Domain.Users;
using Auth.Persistence;
using Blocks.Core.Extensions;
using EmailService.Contracts;
using EmailService.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var checks = 0;
void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
    checks++;
}

Check(Array.Empty<int>().IsEmpty(), "Empty collections must report empty.");
Check(!new[] { 1 }.IsEmpty(), "Nonempty collections must not report empty.");
Check(((IEnumerable<int>?)null).IsNullOrEmpty(), "Null collections must report empty.");
Check(Array.Empty<int>().IsNullOrEmpty(), "Empty collections must report null-or-empty.");
Check(!new[] { 1 }.IsNullOrEmpty(), "Nonempty collections must not report null-or-empty.");

CreateUserCommand CreateCommand(IReadOnlyList<UserRoleDto> roles) => new()
{
    Email = "author@example.com", FirstName = "Ada <Test>", LastName = "Lovelace",
    Gender = Gender.Female, Position = " Researcher ", CompanyName = " Institute ",
    Affiliation = " Mathematics ", UserRoles = roles
};
var user = User.Create(CreateCommand([new(UserRoleType.AUT, null, null)]));
Check(user.Email == "author@example.com", "User creation must preserve email.");
Check(user.Person.FullName == "Ada <Test> Lovelace", "User creation must initialize Person.");
Check(user.Person.ProfessionalProfile?.Position == "Researcher", "Position must not be swapped with affiliation.");
Check(user.Person.ProfessionalProfile?.Affiliation == "Mathematics", "Affiliation must map to affiliation.");
try
{
    User.Create(CreateCommand([]));
    throw new InvalidOperationException("User creation accepted an empty role list.");
}
catch (ArgumentException) { checks++; }

using var db = new AuthDBContext(new DbContextOptionsBuilder<AuthDBContext>()
    .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ArticlesRegression;Trusted_Connection=True").Options);
var userType = db.Model.FindEntityType(typeof(User))!;
var personNavigation = userType.FindNavigation(nameof(User.Person))!;
Check(personNavigation.ForeignKey.IsRequiredDependent, "Person must be a required owned profile.");
var personType = personNavigation.TargetEntityType;
var table = StoreObjectIdentifier.Table(userType.GetTableName()!, userType.GetSchema());
Check(personType.FindProperty("FirstName")!.GetColumnName(table) == "FirstName", "Keep the existing FirstName column.");
Check(personType.FindProperty("Gender")!.GetColumnName(table) == "Gender", "Keep the existing Gender column.");
Check(db.Database.GenerateCreateScript().Contains("AspNetUsers"), "The complete relational model must generate SQL.");

var handler = new SendConfirmationEmailOnUserCreatedHandler(null!, Options.Create(new EmailOptions()), new HttpContextAccessor());
var mail = handler.BuildConfirmationEmail(user, "https://example.com/password?token=a&b=c", "sender@example.com");
var mime = mail.ToMailKitMessage();
Check(mime.From.Mailboxes.Single().Address == "sender@example.com", "Sender address and name must not be reversed.");
Check(mime.To.Mailboxes.Single().Address == user.Email, "Recipient address and name must not be reversed.");
Check(mime.HtmlBody?.Contains("Ada &lt;Test&gt;") == true, "Names must be HTML encoded.");
Check(mime.HtmlBody?.Contains("https://example.com/password?token=a&amp;b=c") == true, "The email must contain an encoded reset link.");

var jwtOptions = new JwtOptions
{
    Issuer = "regression", Audience = "regression", ValidForMinutes = 5,
    SecretKey = "regression-test-only-key-at-least-32-bytes-long"
};
var token = new TokenFactory(Options.Create(jwtOptions), new HttpContextAccessor())
    .GenerateJwtToken(user, ["AUT"], []);
var principal = new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
{
    ValidIssuer = jwtOptions.Issuer, ValidAudience = jwtOptions.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
    ValidateIssuerSigningKey = true, ValidateLifetime = true
}, out _);
Check(principal.Identity?.Name == user.Person.FullName, "Signed tokens must carry the person's name.");
Check(principal.IsInRole("AUT"), "Signed tokens must carry roles.");
var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
{
    ["JwtOptions:Issuer"] = jwtOptions.Issuer,
    ["JwtOptions:Audience"] = jwtOptions.Audience,
    ["JwtOptions:SecretKey"] = jwtOptions.SecretKey,
    ["JwtOptions:ValidForMinutes"] = "5"
}).Build();
var services = new ServiceCollection();
services.AddLogging();
Article.Security.ConfigureAuthentication.AddAuthentication(services, configuration);
using var provider = services.BuildServiceProvider();
var scheme = provider.GetRequiredService<IAuthenticationSchemeProvider>()
    .GetDefaultAuthenticateSchemeAsync().GetAwaiter().GetResult();
Check(scheme?.HandlerType == typeof(JwtBearerHandler), "Default authentication must use the JWT bearer handler.");
var bearerOptions = provider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().Get("Bearer");
var authenticated = new JwtSecurityTokenHandler().ValidateToken(token, bearerOptions.TokenValidationParameters, out _);
Check(authenticated.Identity?.IsAuthenticated == true, "API authentication must accept tokens from TokenFactory.");
Console.WriteLine($"Passed {checks} regression checks (no database or SMTP connection required).");
