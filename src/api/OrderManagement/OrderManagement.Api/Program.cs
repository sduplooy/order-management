using System.Runtime.CompilerServices;
using OrderManagement.Api.Application.Composition;
using OrderManagement.Api.Infrastructure.Composition;
using OrderManagement.Api.Infrastructure.Database;
using OrderManagement.Api.Web.Handlers;

[assembly: InternalsVisibleTo("OrderManagement.Api.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddHsts(opts => { opts.MaxAge = TimeSpan.FromDays(365); })
    .AddCors()
    .AddOpenApi()
    .AddOrderManagementApplication()
    .AddProblemDetails()
    .AddExceptionHandler<KnownExceptionsHandler>()
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

app.UseCors(policyBuilder => policyBuilder
    .WithOrigins("http://localhost:4200")
    .WithMethods("GET", "POST", "PUT", "DELETE")
    .AllowAnyHeader()
);

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.MapHealthChecks("health");

app.MapControllers();

await app
    .InitializeDatabase()
    .RunAsync();
    