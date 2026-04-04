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
   new Producto { Id = 1, Nombre = "Remera gris", Stock = 10, Precio = 3000, Categoria = "Remeras", Tipo = "RemerasLisas", Imagen = "remerablanca.jpeg" },
   new Producto { Id = 2, Nombre = "Remera blanca", Stock = 5, Precio = 6000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeragris.jpeg" },
   new Producto { Id = 3, Nombre = "Remera azul", Stock = 2, Precio = 8000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeraazul.jpeg" },
   new Producto { Id = 4, Nombre = "Remera oso", Stock = 2, Precio = 8000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeraoso.jpeg" },


   // ABRIGOS
   new Producto { Id = 5, Nombre = "Campera 1", Stock = 10, Precio = 3000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera1.jpeg" },
   new Producto { Id = 6, Nombre = "Campera 2", Stock = 5, Precio = 6000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera2.jpeg" },
   new Producto { Id = 7, Nombre = "Campera 3", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera3.jpeg" },
   new Producto { Id = 8, Nombre = "Chaleco", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Chalecos", Imagen = "chaleco.jpeg" },
   
   
   // PANTALONES
   new Producto { Id = 9, Nombre = "Campera 1", Stock = 10, Precio = 3000, Categoria = "Pantalones", Tipo = "Jeans", Imagen = "campera1.jpeg" },
   new Producto { Id = 10, Nombre = "Campera 2", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Algodon", Imagen = "campera2.jpeg" },

   // CORTOS
   new Producto { Id = 11, Nombre = "Campera 1", Stock = 10, Precio = 3000, Categoria = "Cortos", Tipo = "Bermudas", Imagen = "campera1.jpeg" },
   new Producto { Id = 12, Nombre = "Campera 2", Stock = 5, Precio = 6000, Categoria = "Cortos", Tipo = "Shorts", Imagen = "campera2.jpeg" },
   
    // ACCESORIOS
    new Producto { Id = 13, Nombre = "Gorra Nike", Stock = 10, Precio = 3000, Categoria = "Accesorios", Tipo = "Gorros", Imagen = "gorra.jpeg" },
    new Producto { Id = 14, Nombre = "Riñonera Negra", Stock = 5, Precio = 4500, Categoria = "Accesorios", Tipo = "Riñoneras", Imagen = "rinonera.jpeg" }

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


public IActionResult Categoria(string tipo, string filtro)
{
    ViewBag.Tipo = tipo;
    ViewBag.Filtro = filtro;

    var lista = productos.Where(p => p.Categoria == tipo);

    if (!string.IsNullOrEmpty(filtro))
    {
        lista = lista.Where(p => p.Tipo == filtro);
    }

    return View(lista.ToList());
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
