using System.Runtime.CompilerServices;
using OrderManagement.Api.Application.Composition;
using OrderManagement.Api.Infrastructure.Composition;
using OrderManagement.Api.Infrastructure.Database;
using OrderManagement.Api.Web.Handlers;

[assembly: InternalsVisibleTo("OrderManagement.Api.UnitTests")]

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddOpenApi()
    .AddHsts(opts => { opts.MaxAge = TimeSpan.FromDays(365); })
    .AddOrderManagementApplication()
    .AddProblemDetails()
    .AddExceptionHandler<ValidationExceptionHandler>()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddDbContext<OrderManagementDbContext>()
    .AddDatabaseContext(builder.Configuration)
    .AddControllers();

builder.Services
    .AddHealthChecks()
    .AddDatabaseServerHealthCheck(builder.Configuration);

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapHealthChecks("health");

app.MapControllers();

await app
    .InitializeDatabase()
    .RunAsync();
    