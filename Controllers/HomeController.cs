using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoTaller6.Models;
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

    public IActionResult Inventario1()
    {
        var categorias = _context.Categorias.ToList();

    return View(categorias);
}
    

public IActionResult Categoria(string tipo, string filtro)
{
    if (string.IsNullOrEmpty(tipo))
        return RedirectToAction("Inventario1");

    ViewBag.Tipo = tipo;
    ViewBag.Filtro = filtro;

    var lista = _context.Productos
        .Where(p => p.Categoria != null &&
            p.Categoria != null &&
            p.Categoria.Trim().ToLower() == tipo.Trim().ToLower());

    if (!string.IsNullOrEmpty(filtro))
    {
        lista = lista.Where(p =>
            p.Tipo != null &&
            p.Tipo.Trim().ToLower() == filtro.Trim().ToLower());
    }

    // 🔥 FIX ACA  🔥 TRAER SUBCATEGORIAS BIEN NORMALIZADAS
    ViewBag.Subcategorias = _context.Subcategorias
        .Where(s => s.Categoria != null &&
                    s.Categoria.Trim().ToLower() == tipo.Trim().ToLower())
        .ToList();

    return View(lista.ToList());
}



         
     

    // ✅ CREAR CATEGORIA
  [HttpPost]
public JsonResult CrearCategoria([FromBody] Categoria nueva)
{
    // 🔴 VALIDAR VACÍO
    if (string.IsNullOrWhiteSpace(nueva.Nombre) || string.IsNullOrWhiteSpace(nueva.Imagen))
    {
        return Json(new { ok = false, mensaje = "Completar nombre e imagen" });
    }

    nueva.Nombre = nueva.Nombre.Trim();
    nueva.Imagen = nueva.Imagen.Trim();

    // 🔴 EVITAR DUPLICADOS (ignorando mayúsculas)
    bool existe = _context.Categorias
        .Any(c => c.Nombre.ToLower() == nueva.Nombre.ToLower());

    if (existe)
    {
        return Json(new { ok = false, mensaje = "Ya existe esa categoría" });
    }

    _context.Categorias.Add(nueva);
    _context.SaveChanges();

    return Json(new { ok = true });
}



    // ✅ CREAR SUBCATEGORIA (🔥 CLAVE)
  [HttpPost]
public JsonResult CrearSubcategoria([FromBody] Subcategoria nueva)
{
    // 🔴 VALIDAR VACÍO
    if (string.IsNullOrWhiteSpace(nueva.Nombre) || string.IsNullOrWhiteSpace(nueva.Categoria))
    {
        return Json(new { ok = false, mensaje = "Completar datos" });
    }

    nueva.Nombre = nueva.Nombre.Trim();
    nueva.Categoria = nueva.Categoria.Trim();

    // 🔴 EVITAR DUPLICADOS (case insensitive)
    bool existe = _context.Subcategorias
        .Any(s => s.Nombre.ToLower() == nueva.Nombre.ToLower() &&
                  s.Categoria.ToLower() == nueva.Categoria.ToLower());

    if (existe)
    {
        return Json(new { ok = false, mensaje = "Ya existe esa subcategoría" });
    }

    _context.Subcategorias.Add(nueva);
    _context.SaveChanges();

    return Json(new { ok = true });
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

        // 🔥 IMPORTANTE (esto te faltaba)
    existente.Tipo = prod.Tipo;
    existente.Categoria = prod.Categoria;

        _context.SaveChanges();

        return RedirectToAction("Categoria", new { tipo = existente.Categoria });
    }

    [HttpPost]
public JsonResult EliminarProducto(int id)
{
    var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

    if (producto == null)
        return Json(new { ok = false });

    _context.Productos.Remove(producto);
    _context.SaveChanges();

    return Json(new { ok = true });
}

    public IActionResult EscaneoCodigo()
    {
        return View();
    }

    public JsonResult BuscarPorCodigo(string codigo)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Codigo == codigo);
        return Json(new { encontrado = producto != null });
    }

    public JsonResult GenerarCodigoProducto(int id)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Id == id);

        if (producto == null)
            return Json(new { ok = false });

        if (!string.IsNullOrEmpty(producto.Codigo))
            return Json(new { ok = true, codigo = producto.Codigo });

        string codigo = "PROD" + producto.Id.ToString("000");

        producto.Codigo = codigo;
        _context.SaveChanges();

        return Json(new { ok = true, codigo = codigo });
    }

    // ✅ CREAR PRODUCTO (🔥 TAMBIÉN NORMALIZADO)
    [HttpPost]
    public JsonResult CrearProducto([FromBody] Producto nuevo)
    {
        nuevo.Nombre = nuevo.Nombre?.Trim();
        nuevo.Categoria = nuevo.Categoria?.Trim(); // 🔥
        nuevo.Tipo = nuevo.Tipo?.Trim();           // 🔥
        nuevo.Imagen = nuevo.Imagen?.Trim();

        _context.Productos.Add(nuevo);
        _context.SaveChanges();

        return Json(new { ok = true });
    }

    public IActionResult Inventario2(string codigo)
    {
        var producto = _context.Productos.FirstOrDefault(p => p.Codigo == codigo);

        if (producto != null && producto.Stock > 0)
        {
            producto.Stock--;
            _context.SaveChanges();
        }

        return View(producto);
    }

    public IActionResult Error()
    {
        return View();
    }
}