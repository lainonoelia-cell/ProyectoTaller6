using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoTaller6.Models;

namespace ProyectoTaller6.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]




    public IActionResult Inventario1()
{
    return View();
}

public IActionResult EscaneoCodigo()
{
    return View();
}

public IActionResult Inventario2()
{
    return View();
}

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
