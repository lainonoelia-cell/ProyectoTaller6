using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoTaller6.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoTaller6.Controllers;



public class HomeController : Controller
{

public static List<Producto> productos = new List<Producto>
{
    // REMERAS
    new Producto { Id = 1, Nombre = "Remera gris", Stock = 10, Precio = 5000, Categoria = "Remeras", Imagen = "remeragris.jpeg" },
    new Producto { Id = 2, Nombre = "Remera blanca", Stock = 5, Precio = 6000, Categoria = "Remeras", Imagen = "remerablanca.jpeg" },
    new Producto { Id = 3, Nombre = "Remera azul", Stock = 2, Precio = 8000, Categoria = "Remeras", Imagen = "remeraazul.jpeg" },

    // ABRIGOS
    new Producto { Id = 4, Nombre = "Campera negra", Stock = 4, Precio = 15000, Categoria = "Abrigos", Imagen = "campera1.jpeg" },
    new Producto { Id = 5, Nombre = "Campera verde", Stock = 3, Precio = 18000, Categoria = "Abrigos", Imagen = "campera2.jpeg" },
    new Producto { Id = 5, Nombre = "Campera verde", Stock = 3, Precio = 18000, Categoria = "Abrigos", Imagen = "campera3.jpeg" },
    new Producto { Id = 5, Nombre = "Chaleco", Stock = 3, Precio = 18000, Categoria = "Abrigos", Imagen = "chaleco.jpeg" },
    
    // PANTALONES
    new Producto { Id = 6, Nombre = "Pantalón jean", Stock = 6, Precio = 12000, Categoria = "Pantalones", Imagen = "pantalon1.jpeg" },

    // BERMUDAS
    new Producto { Id = 7, Nombre = "Bermuda gris", Stock = 8, Precio = 9000, Categoria = "Bermudas", Imagen = "bermuda1.jpeg" },

    // ACCESORIOS
    new Producto { Id = 8, Nombre = "Gorra", Stock = 15, Precio = 3000, Categoria = "Accesorios", Imagen = "gorra.jpeg" }
};
    
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


public IActionResult Categoria(string tipo)
{
    ViewBag.Tipo = tipo; // 👈 ESTA LÍNEA FALTABA

    var lista = productos.Where(p => p.Categoria == tipo).ToList();
    return View(lista);
}






public IActionResult EditarProducto(int id)
{
    var producto = productos.First(p => p.Id == id);
    return View(producto);
}



[HttpPost]
public IActionResult GuardarProducto(Producto prod)
{
    var existente = productos.First(p => p.Id == prod.Id);

    existente.Nombre = prod.Nombre;
    existente.Stock = prod.Stock;
    existente.Precio = prod.Precio;

    return RedirectToAction("Categoria", new { tipo = existente.Categoria });
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
