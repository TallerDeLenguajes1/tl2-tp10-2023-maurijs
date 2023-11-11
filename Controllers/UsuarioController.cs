using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp10.Models;

namespace tp10.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public UsuarioController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
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
