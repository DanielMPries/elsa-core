using Elsa;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var elsaSection = builder.Configuration.GetSection("Elsa");

// Elsa Services
builder.Services.AddElsa(elsa => elsa
    .AddConsoleActivities()
    .AddHttpActivities(elsaSection.GetSection("Server").Bind)
    .AddQuartzTemporalActivities()
    .AddJavaScriptActivities()
    .AddWorkflowsFrom<Program>());

// Elsa API Endpoints
builder.Services.AddElsaApiEndpoints();

// Modify CORS policy to allow requests from all domains
// Do not do this in production!
builder.Services
    .AddCors(cors => cors.AddDefaultPolicy(policy => policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin()
        .WithExposedHeaders("Content-Disposition")));

var app = builder.Build();

app
    .UseCors()
    .UseHttpActivities()
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();