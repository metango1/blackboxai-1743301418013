using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmailManagement.Models;

namespace EmailManagement.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        try
        {
            _logger.LogInformation("Accessing home page");
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while accessing home page");
            return RedirectToAction(nameof(Error));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        _logger.LogError("Error page accessed with RequestId: {RequestId}", requestId);
        return View(new ErrorViewModel { RequestId = requestId });
    }
}