using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

public class HostModel : PageModel {
    private readonly IConfiguration _config;

    public HostModel(IConfiguration config) {
        _config = config;
    }

    public string ServerUrl { get; private set; } = "https://localhost:5000";

    public void OnGet() {
        ServerUrl = _config.GetSection("Elsa:ServerUrl").Value;
    }
}