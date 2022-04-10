using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();
app
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoints => endpoints.MapFallbackToPage("/_Host"));

app.Run();
