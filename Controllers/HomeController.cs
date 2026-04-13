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
   new Producto { Id = 1, Nombre = "Remera Blanca", Stock = 10, Precio = 3000, Categoria = "Remeras", Tipo = "RemerasLisas", Imagen = "remerablanca.jpeg"},
   new Producto { Id = 2, Nombre = "Remera Gris", Stock = 5, Precio = 6000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeragris.jpeg"},
   new Producto { Id = 3, Nombre = "Remera Azul", Stock = 2, Precio = 8000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeraazul.jpeg" },
   new Producto { Id = 4, Nombre = "Remera Oso", Stock = 2, Precio = 8000, Categoria = "Remeras", Tipo = "RemerasEstampadas", Imagen = "remeraoso.jpeg" },


   // ABRIGOS
   new Producto { Id = 5, Nombre = "Campera Impermeable Reversible 1", Stock = 10, Precio = 3000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera1.jpeg" },
   new Producto { Id = 6, Nombre = "Campera Impermeable Reversible 2", Stock = 5, Precio = 6000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera2.jpeg" },
   new Producto { Id = 7, Nombre = "Campera Impermeable Reversible 3", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "campera3.jpeg" },
   new Producto { Id = 8, Nombre = "Chaleco", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Chalecos", Imagen = "chaleco.jpeg" },
   new Producto { Id = 9, Nombre = "Buzo Overside Dados", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Buzos", Imagen = "buzoOverside1.png" },
   new Producto { Id = 10, Nombre = "Buzo Overside Cherries", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Buzos", Imagen = "buzoOverside2.png" },
   new Producto { Id = 11, Nombre = "Campera Original Frisa", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Camperas", Imagen = "camperaFrisa.png" },
   new Producto { Id = 12, Nombre = "Sweater Bremer Azul Marino", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Sweaters", Imagen = "sweaterAzul.png" },
   new Producto { Id = 13, Nombre = "Sweater Bremer Bordo", Stock = 2, Precio = 8000, Categoria = "Abrigos", Tipo = "Sweaters", Imagen = "sweaterBordo.png" },
   
   
   
   
   // PANTALONES
   new Producto { Id = 14, Nombre = "Pantalón Baggy", Stock = 10, Precio = 3000, Categoria = "Pantalones", Tipo = "Jeans", Imagen = "pantalonBaggy.png" },
   new Producto { Id = 15, Nombre = "Deportivo", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Deportivo", Imagen = "deportivo.png" },
   new Producto { Id = 16, Nombre = "Cargo Camuflado", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Cargo", Imagen = "cargoCamuflado.png" },
   new Producto { Id = 17, Nombre = "Cargo con abrojo", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Cargo", Imagen = "cargoAbrojo.png" },
   new Producto { Id = 18, Nombre = "Joggins", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Joggins", Imagen = "joggins.png" },
   new Producto { Id = 19, Nombre = "Gabardina", Stock = 5, Precio = 6000, Categoria = "Pantalones", Tipo = "Gabardina", Imagen = "gabardina.png" },
   new Producto { Id = 20, Nombre = "Chupin Elastizado", Stock = 10, Precio = 3000, Categoria = "Pantalones", Tipo = "Jeans", Imagen = "chupinElastizado.png" },



   // CORTOS
   new Producto { Id = 21, Nombre = "Shorts Deportivos", Stock = 10, Precio = 3000, Categoria = "Cortos", Tipo = "Shorts", Imagen = "shortDeportivo.jpg" },
   new Producto { Id = 22, Nombre = "Short Deportivo Lycra", Stock = 5, Precio = 6000, Categoria = "Cortos", Tipo = "Shorts", Imagen = "shortDeportivoLycra.png" },
   new Producto { Id = 23, Nombre = "Bermudas Camufladas", Stock = 5, Precio = 6000, Categoria = "Cortos", Tipo = "Bermudas", Imagen = "bermudasCamufladas.png" },
   
   
    // ACCESORIOS
    new Producto { Id = 24, Nombre = "Gorra Bordada", Stock = 10, Precio = 3000, Categoria = "Accesorios", Tipo = "Gorros", Imagen = "gorra.png"},
    new Producto { Id = 25, Nombre = "Riñoneras Varias", Stock = 5, Precio = 4500, Categoria = "Accesorios", Tipo = "Riñoneras", Imagen = "riñoneras.png" }

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



public JsonResult BuscarPorCodigo(string codigo)
{
    var producto = productos.FirstOrDefault(p => p.Codigo == codigo);

    if (producto != null)
    {
        return Json(new { encontrado = true });
    }

    return Json(new { encontrado = false });
}


public JsonResult GenerarCodigoProducto(int id)
{
    var producto = productos.FirstOrDefault(p => p.Id == id);

    if (producto == null)
        return Json(new { ok = false });

    // si ya tiene código no lo pisa
    if (!string.IsNullOrEmpty(producto.Codigo))
    {
        return Json(new { ok = true, codigo = producto.Codigo });
    }

    // generar código automático
    string codigo = "PROD" + producto.Id.ToString("000");

    producto.Codigo = codigo;

    return Json(new { ok = true, codigo = codigo });
}


[HttpPost]
public JsonResult CrearProducto([FromBody] Producto nuevo)
{
    nuevo.Id = productos.Count + 1;

    productos.Add(nuevo);

    return Json(new { ok = true });
}


public IActionResult Inventario2(string codigo)
{
    var producto = productos.FirstOrDefault(p => p.Codigo == codigo);

    if (producto != null)
    {
        if (producto.Stock > 0)
        {
            producto.Stock--; // 🔥 ACA SE DESCuenta
        }
    }

    return View(producto);
}


    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
