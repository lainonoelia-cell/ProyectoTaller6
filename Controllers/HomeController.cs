using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoTaller6.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoTaller6.Controllers;



public class HomeController : Controller
{

private readonly AppDbContext _context;

public HomeController(AppDbContext context)
{
    _context = context;
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




    public IActionResult Inventario1()
{
    return View();
}


public IActionResult Categoria(string tipo, string filtro)
{
    ViewBag.Tipo = tipo;
    ViewBag.Filtro = filtro;

   var lista = _context.Productos.Where(p => p.Categoria == tipo);

    if (!string.IsNullOrEmpty(filtro))
    {
        lista = lista.Where(p => p.Tipo == filtro);
    }

    return View(lista.ToList());
}






public IActionResult EditarProducto(int id)
{
   var producto = _context.Productos.First(p => p.Id == id);
    return View(producto);
}


[HttpPost]
public IActionResult GuardarProducto(Producto prod)
{
    var existente = _context.Productos.First(p => p.Id == prod.Id);

    existente.Nombre = prod.Nombre;
    existente.Stock = prod.Stock;
    existente.Precio = prod.Precio;

    _context.SaveChanges(); // 🔥

    return RedirectToAction("Categoria", new { tipo = existente.Categoria });
}




public IActionResult EscaneoCodigo()
{
    return View();
}



public JsonResult BuscarPorCodigo(string codigo)
{
    var producto = _context.Productos.FirstOrDefault(p => p.Codigo == codigo);

    if (producto != null)
    {
        return Json(new { encontrado = true });
    }

    return Json(new { encontrado = false });
}


public JsonResult GenerarCodigoProducto(int id)
{
    var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

    if (producto == null)
        return Json(new { ok = false });

    if (!string.IsNullOrEmpty(producto.Codigo))
    {
        return Json(new { ok = true, codigo = producto.Codigo });
    }

    string codigo = "PROD" + producto.Id.ToString("000");

    producto.Codigo = codigo;

    _context.SaveChanges(); // 🔥

    return Json(new { ok = true, codigo = codigo });
}



[HttpPost]
public JsonResult CrearProducto([FromBody] Producto nuevo)
{
    _context.Productos.Add(nuevo);
_context.SaveChanges();

    return Json(new { ok = true });
}


public IActionResult Inventario2(string codigo)
{
   var producto = _context.Productos.FirstOrDefault(p => p.Codigo == codigo);

if (producto != null)
{
    if (producto.Stock > 0)
    {
        producto.Stock--;
        _context.SaveChanges(); // 🔥
    }
}
    

    return View(producto);
}


    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
