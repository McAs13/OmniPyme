using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniPyme.Models;
using OmniPyme.Web.Core.Pagination;
using OmniPyme.Web.Services;
using Serilog;

namespace OmniPyme.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        Log.Warning("Log de advertencia");
        Log.Error("Log de error");
        Log.Fatal("Log fatal");
        Log.Information("Log de informaci�n");
        Log.Debug("Log de depuraci�n");

        try
        {
            int a = 13;
            int b = 0;
            int c = a / b; // Esto generar� una excepci�n de divisi�n por cero

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Ha ocurrido un error en HomeController.Index");
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
