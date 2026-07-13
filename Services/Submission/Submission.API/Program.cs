using Submission.API;
using Submission.API.Endpoints;
using Submission.Application;
using Submission.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add services
builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);
#endregion

var app = builder.Build();

#region Use services
app.UseSwagger()
    .UseSwaggerUI()
    .UseRouting();
app.MapAllEndpoints();
// todo
// Create First Migration and Update Database
if (app.Environment.IsDevelopment())
{
}
#endregion

app.MapAllEndpoints();
app.Run();
