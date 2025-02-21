using System.Runtime.CompilerServices;
using OrderManagement.Api.Handlers;
using OrderManagement.Api.Infrastructure.Composition;
using OrderManagement.Api.Infrastructure.Database;

[assembly: InternalsVisibleTo("OrderManagement.Api.UnitTests")]

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddOpenApi()
    .AddHsts(opts => { opts.MaxAge = TimeSpan.FromDays(365); })
    .AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddDbContext<OrderManagementDbContext>()
    .AddDatabaseContext(builder.Configuration)
    .AddHealthChecks()
        .AddDatabaseServerHealthCheck(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapHealthChecks("health");

app.UseHsts();
app.UseHttpsRedirection();

await app
    .InitializeDatabase()
    .RunAsync();