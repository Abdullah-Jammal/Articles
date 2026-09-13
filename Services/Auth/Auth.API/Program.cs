using Auth.API;
using Auth.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiOptions(builder.Configuration);

#region Add services
builder.Services
    .AddApiService(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);
#endregion

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
