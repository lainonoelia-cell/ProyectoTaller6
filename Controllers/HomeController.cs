using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoTaller6.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ProyectoTaller6.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public HomeController(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View("Bienvenida");
    }
 
    public IActionResult Bienvenida()
    {
    return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
    return View();
    }

    public IActionResult RegistroRedSocial(string red)
    {
    ViewBag.Red = red;
    return View();
    }

[HttpPost]
public IActionResult RegistroRedSocial(string red, string usuarioRed)
{
    // ==========================================
    // VALIDAR RED SOCIAL
    // ==========================================

    if (string.IsNullOrWhiteSpace(red))
    {
        ViewBag.Red = "Red Social";
        ViewBag.Error = "No se seleccionó una red social.";
        return View();
    }

    // ==========================================
    // VALIDAR USUARIO
    // ==========================================

    if (string.IsNullOrWhiteSpace(usuarioRed))
    {
        ViewBag.Red = red;
        ViewBag.Error = "Por favor, ingresá tu nombre de usuario.";
        return View();
    }

    usuarioRed = usuarioRed.Trim();
    red = red.Trim();

    if (red != "Facebook" &&
    red != "Instagram" &&
    red != "LinkedIn")
{
    ViewBag.Red = "Red Social";
    ViewBag.Error = "Elegí una red social válida.";
    return View();
}

    // ==========================================
    // VALIDAR CARACTERES
    // ==========================================

    bool usuarioValido =
        System.Text.RegularExpressions.Regex.IsMatch(
            usuarioRed,
            @"^[a-zA-Z0-9._]+$"
        );

    if (!usuarioValido)
    {
        ViewBag.Red = red;
        ViewBag.Error =
            "El usuario contiene caracteres no permitidos. " +
            "Usá solamente letras, números, puntos o guiones bajos.";

        return View();
    }

    // ==========================================
    // BUSCAR CLIENTE EXISTENTE
    // ==========================================

    var cliente = _context.Clientes
        .FirstOrDefault(c =>
            c.UsuarioRed.ToLower() == usuarioRed.ToLower() &&
            c.RedSocial.ToLower() == red.ToLower());

    // ==========================================
    // CREAR CLIENTE SOLAMENTE SI NO EXISTE
    // ==========================================

    if (cliente == null)
    {
        cliente = new Cliente
        {
            UsuarioRed = usuarioRed,
            RedSocial = red,
            FechaRegistro = DateTime.Now
        };

        _context.Clientes.Add(cliente);
        _context.SaveChanges();
    }

    // ==========================================
    // GUARDAR CLIENTE EN SESIÓN
    // ==========================================

 HttpContext.Session.Clear();

HttpContext.Session.SetInt32("ClienteId", cliente.Id);
HttpContext.Session.SetString("UsuarioRed", cliente.UsuarioRed);
HttpContext.Session.SetString("RedSocial", cliente.RedSocial);
HttpContext.Session.SetString("Rol", "Cliente");

    return RedirectToAction("GestionCliente");
}







[HttpPost]
public IActionResult Login(string usuario, string password)
{
    var adminUsuario = _configuration["Admin:Username"];
    var adminPassword = _configuration["Admin:Password"];

    if (!string.IsNullOrWhiteSpace(adminUsuario) &&
        !string.IsNullOrWhiteSpace(adminPassword) &&
        usuario == adminUsuario &&
        password == adminPassword)
    {
        HttpContext.Session.Clear();
        HttpContext.Session.SetString("Rol", "Admin");

        TempData["LoginCorrecto"] = true;

        return RedirectToAction("Inventario1");
    }

    ViewBag.Error = "Usuario o contraseña incorrectos";
    return View();
}

    public IActionResult Inventario1()
{
    if (HttpContext.Session.GetString("Rol") != "Admin")
    {
        return RedirectToAction("Bienvenida");
    }

    var categorias = _context.Categorias.ToList();
    return View(categorias);
}

    public IActionResult Tienda()
    {
    return View();
    }
    

public IActionResult Categoria(string tipo, string filtro)
{
    if (HttpContext.Session.GetString("Rol") != "Admin")
    {
        return RedirectToAction("Bienvenida");
    }
    
    if (string.IsNullOrEmpty(tipo))
        return RedirectToAction("Inventario1");

    ViewBag.Tipo = tipo;
    ViewBag.Filtro = filtro;

    var lista = _context.Productos
    .Where(p => p.Categoria != null &&
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


return View(lista
    .OrderBy(p => p.OrdenCatalogo)
    .ToList());

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

[HttpGet]
public IActionResult Prueba()
{
    return Content("ESTOY EN EL HOMECONTROLLER CORRECTO");
}

// ==========================================
// EDITAR CATEGORIA
// ==========================================
[HttpPut]
public JsonResult EditarCategoria([FromBody] Categoria categoria)
{
    // ==========================================
    // VALIDAR DATOS
    // ==========================================

    if (string.IsNullOrWhiteSpace(categoria.Nombre) ||
        string.IsNullOrWhiteSpace(categoria.Imagen))
    {
        return Json(new
        {
            ok = false,
            mensaje = "Debe ingresar el nombre y la imagen."
        });
    }

    categoria.Nombre = categoria.Nombre.Trim();
    categoria.Imagen = categoria.Imagen.Trim();

    // ==========================================
    // BUSCAR CATEGORIA
    // ==========================================

    var existente = _context.Categorias
        .FirstOrDefault(c => c.Id == categoria.Id);

    if (existente == null)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Categoría no encontrada."
        });
    }

    // ==========================================
    // EVITAR CATEGORIAS DUPLICADAS
    // ==========================================

    bool repetida = _context.Categorias.Any(c =>
        c.Id != categoria.Id &&
        c.Nombre.Trim().ToLower() ==
        categoria.Nombre.Trim().ToLower());

    if (repetida)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Ya existe una categoría con ese nombre."
        });
    }

    // Guardamos el nombre anterior
    string nombreAnterior = existente.Nombre;

    // ==========================================
    // ACTUALIZAR CATEGORIA
    // ==========================================

    existente.Nombre = categoria.Nombre;
    existente.Imagen = categoria.Imagen;

    // ==========================================
    // ACTUALIZAR PRODUCTOS
    // ==========================================

    var productos = _context.Productos
        .Where(p => p.Categoria != null &&
                    p.Categoria.Trim().ToLower() ==
                    nombreAnterior.Trim().ToLower())
        .ToList();

    foreach (var p in productos)
    {
        p.Categoria = categoria.Nombre;
    }

    // ==========================================
    // ACTUALIZAR SUBCATEGORIAS
    // ==========================================

    var subcategorias = _context.Subcategorias
        .Where(s => s.Categoria != null &&
                    s.Categoria.Trim().ToLower() ==
                    nombreAnterior.Trim().ToLower())
        .ToList();

    foreach (var s in subcategorias)
    {
        s.Categoria = categoria.Nombre;
    }

    // ==========================================
    // GUARDAR
    // ==========================================

    _context.SaveChanges();

    return Json(new
    {
        ok = true,
        mensaje = "Categoría actualizada correctamente."
    });
}

// ==========================================
// ELIMINAR CATEGORIA
// ==========================================
[HttpDelete]
public JsonResult EliminarCategoria(int id)
{
    try
    {
        var categoria = _context.Categorias
            .FirstOrDefault(c => c.Id == id);

        if (categoria == null)
        {
            return Json(new
            {
                ok = false,
                mensaje = "Categoría no encontrada."
            });
        }

        bool tieneSubcategorias = _context.Subcategorias.Any(s =>
            s.Categoria != null &&
            s.Categoria.Trim().ToLower() ==
            categoria.Nombre.Trim().ToLower());

        if (tieneSubcategorias)
        {
            return Json(new
            {
                ok = false,
                mensaje = "Debe eliminar primero las subcategorías."
            });
        }

        bool tieneProductos = _context.Productos.Any(p =>
            p.Categoria != null &&
            p.Categoria.Trim().ToLower() ==
            categoria.Nombre.Trim().ToLower());

        if (tieneProductos)
        {
            return Json(new
            {
                ok = false,
                mensaje = "Debe eliminar primero los productos de esta categoría."
            });
        }

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();

        return Json(new
        {
            ok = true,
            mensaje = "Categoría eliminada correctamente."
        });
    }
    catch (Exception ex)
    {
        return Json(new
        {
            ok = false,
            mensaje = ex.ToString()
        });
    }
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

// ==========================================
// EDITAR SUBCATEGORIA
// ==========================================
[HttpPut]
public JsonResult EditarSubcategoria([FromBody] Subcategoria subcategoria)
{
    if (string.IsNullOrWhiteSpace(subcategoria.Nombre))
    {
        return Json(new
        {
            ok = false,
            mensaje = "Debe ingresar un nombre."
        });
    }

    subcategoria.Nombre = subcategoria.Nombre.Trim();

    var existente = _context.Subcategorias
        .FirstOrDefault(s => s.Id == subcategoria.Id);

    if (existente == null)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Subcategoría no encontrada."
        });
    }

    bool repetida = _context.Subcategorias.Any(s =>
        s.Id != subcategoria.Id &&
        s.Categoria.Trim().ToLower() == existente.Categoria.Trim().ToLower() &&
        s.Nombre.Trim().ToLower() == subcategoria.Nombre.Trim().ToLower());

    if (repetida)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Ya existe esa subcategoría."
        });
    }

    string nombreAnterior = existente.Nombre;

    existente.Nombre = subcategoria.Nombre;

    // Actualizar productos que pertenecen a esa subcategoría
    var productos = _context.Productos
        .Where(p => p.Tipo.Trim().ToLower() ==
                    nombreAnterior.Trim().ToLower())
        .ToList();

    foreach (var p in productos)
    {
        p.Tipo = subcategoria.Nombre;
    }

    _context.SaveChanges();

    return Json(new
    {
        ok = true,
        mensaje = "Subcategoría actualizada correctamente."
    });
}

// ==========================================
// ELIMINAR SUBCATEGORIA
// ==========================================
[HttpDelete]
public JsonResult EliminarSubcategoria(int id)
{
    var subcategoria = _context.Subcategorias
        .FirstOrDefault(s => s.Id == id);

    if (subcategoria == null)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Subcategoría no encontrada."
        });
    }

    bool tieneProductos = _context.Productos.Any(p =>
        p.Tipo.Trim().ToLower() ==
        subcategoria.Nombre.Trim().ToLower());

    if (tieneProductos)
    {
        return Json(new
        {
            ok = false,
            mensaje = "Debe eliminar primero los productos de esta subcategoría."
        });
    }

    _context.Subcategorias.Remove(subcategoria);

    _context.SaveChanges();

    return Json(new
    {
        ok = true,
        mensaje = "Subcategoría eliminada correctamente."
    });
}

   // ✅ CREAR PRODUCTO (🔥 TAMBIÉN NORMALIZADO)
    [HttpPost]
public JsonResult CrearProducto([FromBody] Producto producto)
{
    try
    {
        // Verificar si ya existe un producto con el mismo nombre,
        // categoría y subcategoría.
        bool existe = _context.Productos.Any(p =>
            p.Nombre.ToLower().Trim() == producto.Nombre.ToLower().Trim() &&
            p.Categoria.ToLower().Trim() == producto.Categoria.ToLower().Trim() &&
            p.Tipo.ToLower().Trim() == producto.Tipo.ToLower().Trim());

        if (existe)
        {
            return Json(new
            {
                ok = false,
                mensaje = "Ya existe un producto con ese nombre en esta subcategoría."
            });
        }

        _context.Productos.Add(producto);
        _context.SaveChanges();

        return Json(new
        {
            ok = true,
            mensaje = "Producto creado correctamente."
        });
    }
    catch (Exception ex)
    {
        return Json(new
        {
            ok = false,
            mensaje = ex.Message
        });
    }
}


    public IActionResult EditarProducto(int id)
    {

        if (HttpContext.Session.GetString("Rol") != "Admin")
    {
        return RedirectToAction("Bienvenida");
    }
        var producto = _context.Productos.First(p => p.Id == id);
       if (producto == null)
    {
        return NotFound();
    }

    var variantes = _context.ProductoVariantes
        .Where(v => v.ProductoId == id)
        .ToList();

    ViewBag.Variantes = variantes;

    return View(producto);
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
        if (HttpContext.Session.GetString("Rol") != "Admin")
    {
        return RedirectToAction("Bienvenida");
    }
        
        return View();
    }

   public JsonResult BuscarPorCodigo(string codigo)
{
    codigo = codigo?.Trim();

    var producto = _context.Productos
        .FirstOrDefault(p => p.Codigo == codigo);

    if (producto == null)
    {
        return Json(new
        {
            encontrado = false
        });
    }

    return Json(new
    {
        encontrado = true,
        codigo = producto.Codigo,
        nombre = producto.Nombre,
        precio = producto.Precio,
        stock = producto.Stock,
        imagen = producto.Imagen,
        categoria = producto.Categoria
    });
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

 
[HttpGet]
public IActionResult Inventario2(string codigo)
{
    if (HttpContext.Session.GetString("Rol") != "Admin")
    {
        return RedirectToAction("Bienvenida");
    }
    
    // 🧼 LIMPIEZA DEL CÓDIGO (CLAVE)
    codigo = codigo?.Trim();

    if (string.IsNullOrEmpty(codigo))
    {
        return RedirectToAction("Inventario1", new { error = "1" });
    }

    var producto = _context.Productos
        .FirstOrDefault(p => p.Codigo != null && p.Codigo.Trim() == codigo);

    if (producto == null)
    {
        return RedirectToAction("Inventario1", new { error = "1" });
    }

    // ✔ PRODUCTO OK
    TempData["Success"] = $"Producto: {producto.Nombre}";

    if (producto.Stock > 0)
    {
        producto.Stock--;
        _context.SaveChanges();
    }

    return View(producto);
}

   public IActionResult GestionCliente()
{
    if (HttpContext.Session.GetString("Rol") != "Cliente")
    {
        return RedirectToAction("RegistroRedSocial");
    }

    ViewBag.Usuario = HttpContext.Session.GetString("UsuarioRed");
    ViewBag.Red = HttpContext.Session.GetString("RedSocial");

    return View();
}

public IActionResult ProductoDetalle(string categoria, string badge)
{
    
    if (HttpContext.Session.GetString("Rol") != "Cliente")
    {
        return RedirectToAction("RegistroRedSocial");
    }
    // ==========================================
    // DATOS DEL CLIENTE REGISTRADO
    // ==========================================

    var usuarioRed =
        HttpContext.Session.GetString("UsuarioRed") ?? "";

    var redSocial =
        HttpContext.Session.GetString("RedSocial") ?? "";

    ViewBag.Usuario = usuarioRed;
    ViewBag.Red = redSocial;


    // ==========================================
    // PRODUCTOS QUE SE VAN A MOSTRAR
    // ==========================================

    List<Producto> productos;


    // ==========================================
    // SI VIENE DESDE UN BADGE
    // ==========================================

    if (!string.IsNullOrWhiteSpace(badge))
    {
        productos = _context.Productos
            .Where(p =>
                p.Badge != null &&
                p.Badge.Trim()
                    .ToLower()
                    .Contains(badge.Trim().ToLower()))
            .OrderBy(p => p.OrdenBadge)
            .ToList();

        ViewBag.Categoria = badge;
        ViewBag.EsBadge = true;
    }


    // ==========================================
    // SI VIENE DESDE UNA CATEGORÍA
    // ==========================================

    else
    {
        if (string.IsNullOrWhiteSpace(categoria))
        {
            return RedirectToAction("GestionCliente");
        }

        productos = _context.Productos
            .Where(p =>
                p.Categoria != null &&
                p.Categoria.Trim()
                    .ToLower() ==
                categoria.Trim().ToLower())
            .OrderBy(p => p.OrdenCatalogo)
            .ToList();

        ViewBag.Categoria = categoria;
        ViewBag.EsBadge = false;
    }


    // ==========================================
    // BUSCAR VARIANTES DE LOS PRODUCTOS
    // ==========================================

    var productoIds = productos
        .Select(p => p.Id)
        .ToList();


    var variantes = _context.ProductoVariantes
        .Where(v => productoIds.Contains(v.ProductoId))
        .ToList();

        


    ViewBag.Variantes = variantes;


    // ==========================================
    // MOSTRAR LA VISTA
    // ==========================================

    return View(productos);
}

[HttpPost]
public IActionResult GuardarProducto(
    Producto prod,
    string[] VariantesColor,
    string[] VariantesTalle,
    int[] VariantesStock)
{
    var existente = _context.Productos
        .FirstOrDefault(p => p.Id == prod.Id);

    if (existente == null)
    {
        return NotFound();
    }

    // ==========================================
    // PRODUCTO PRINCIPAL
    // ==========================================

    existente.Nombre = prod.Nombre;
    existente.Stock = prod.Stock;
    existente.Precio = prod.Precio;

    existente.Tipo = prod.Tipo;
    existente.Categoria = prod.Categoria;
    existente.Imagen = prod.Imagen;

    existente.Badge = prod.Badge;
    existente.ColorBadge = prod.ColorBadge;

    existente.OrdenCatalogo = prod.OrdenCatalogo;
    existente.OrdenBadge = prod.OrdenBadge;

    // ==========================================
    // COLOR PRINCIPAL
    // ==========================================

    existente.Color =
        string.IsNullOrWhiteSpace(prod.Color)
            ? "Único"
            : prod.Color.Trim();

    // ==========================================
    // ELIMINAR TODAS LAS VARIANTES ANTERIORES
    // ==========================================

    var variantesAnteriores =
        _context.ProductoVariantes
            .Where(v =>
                v.ProductoId == existente.Id)
            .ToList();

    foreach (var variante in variantesAnteriores)
    {
        _context.ProductoVariantes.Remove(variante);
    }


    Console.WriteLine("======================================");
Console.WriteLine("COLORES RECIBIDOS: " + (VariantesColor?.Length ?? 0));
Console.WriteLine("TALLES RECIBIDOS: " + (VariantesTalle?.Length ?? 0));
Console.WriteLine("STOCKS RECIBIDOS: " + (VariantesStock?.Length ?? 0));

if (VariantesColor != null)
{
    for (int i = 0; i < VariantesColor.Length; i++)
    {
        Console.WriteLine(
            $"COLOR [{i}]: {VariantesColor[i]}"
        );
    }
}

Console.WriteLine("======================================");

    // ==========================================
    // CREAR NUEVAS VARIANTES
    // ==========================================

    if (
        VariantesColor != null &&
        VariantesTalle != null &&
        VariantesStock != null
    )
    {
        var cantidad =
            Math.Min(
                VariantesColor.Length,
                Math.Min(
                    VariantesTalle.Length,
                    VariantesStock.Length
                )
            );

        for (int i = 0; i < cantidad; i++)
        {
            var color =
                VariantesColor[i]?.Trim();

            var talle =
                VariantesTalle[i]?.Trim();

            var stock =
                VariantesStock[i];

            if (
                string.IsNullOrWhiteSpace(color) ||
                string.IsNullOrWhiteSpace(talle)
            )
            {
                continue;
            }

            var nuevaVariante =
                new ProductoVariante
                {
                    ProductoId =
                        existente.Id,

                    Color =
                        color,

                    Talle =
                        talle,

                    Stock =
                        stock
                };

            _context.ProductoVariantes.Add(
                nuevaVariante
            );
        }
    }

    // ==========================================
    // GUARDAR
    // ==========================================

    _context.SaveChanges();

    return RedirectToAction(
        "Categoria",
        new
        {
            tipo = existente.Categoria
        });
}
[HttpGet]
public JsonResult ObtenerProductos(string categoria)
{
    var productos = _context.Productos
        .Where(p => p.Categoria == categoria)
        .OrderBy(p => p.OrdenCatalogo)
        .Select(p => new
        {
            id = p.Id,
            nombre = p.Nombre,
            precio = p.Precio,
            stock = p.Stock,
            imagen = p.Imagen,
            badge = p.Badge,
            colorBadge = p.ColorBadge
        })
        .ToList();

    return Json(productos);
}


[HttpPost]
public IActionResult AgregarAlCarrito(
    [FromBody] AgregarCarritoRequest request)
{
    // ==========================================
    // OBTENER CLIENTE DE LA SESIÓN
    // ==========================================

    var clienteId =
        HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    // ==========================================
    // BUSCAR CARRITO ACTIVO
    // ==========================================

    var carrito = _context.Carritos
        .FirstOrDefault(c =>
            c.ClienteId == clienteId.Value &&
            c.Estado == "Activo");

    // ==========================================
    // SI NO EXISTE, CREARLO
    // ==========================================

    if (carrito == null)
    {
        carrito = new Carrito
        {
            ClienteId = clienteId.Value,
            Estado = "Activo",
            FechaCreacion = DateTime.Now
        };

        _context.Carritos.Add(carrito);

        _context.SaveChanges();
    }

    // ==========================================
    // BUSCAR SI EL PRODUCTO YA ESTÁ
    // EN EL CARRITO CON EL MISMO TALLE
    // ==========================================

    var detalle = _context.CarritoDetalles
    .FirstOrDefault(d =>
        d.CarritoId == carrito.Id &&
        d.ProductoId == request.ProductoId &&
        d.Talle == request.Talle &&
        d.Color == request.Color);

    // ==========================================
    // SI YA EXISTE → SUMAR CANTIDAD
    // ==========================================

    if (detalle != null)
    {
        detalle.Cantidad += request.Cantidad;

        detalle.Precio = request.Precio;

        detalle.FormaPago = request.FormaPago ?? "";
    }
    else
    {
        // ======================================
        // SI NO EXISTE → CREAR DETALLE
        // ======================================

        detalle = new CarritoDetalle
        {
            CarritoId = carrito.Id,
            ProductoId = request.ProductoId,
              Color = request.Color,
            Talle = request.Talle,
            Cantidad = request.Cantidad,
            Precio = request.Precio,
           FormaPago = request.FormaPago ?? ""
        };

        _context.CarritoDetalles.Add(detalle);
    }

    // ==========================================
    // GUARDAR EN MYSQL
    // ==========================================

    _context.SaveChanges();

    return Json(new
    {
        exito = true,
        mensaje = "Producto agregado al carrito.",
        carritoId = carrito.Id
    });
}   

[HttpPost]
public IActionResult ActualizarFormaPago(
    [FromBody] ActualizarFormaPagoRequest request)
{
    var clienteId =
        HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    var carrito = _context.Carritos
        .FirstOrDefault(c =>
            c.ClienteId == clienteId.Value &&
            c.Estado == "Activo");

    if (carrito == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No existe un carrito activo."
        });
    }

    var detalle = _context.CarritoDetalles
        .FirstOrDefault(d =>
            d.CarritoId == carrito.Id &&
            d.ProductoId == request.ProductoId &&
            d.Talle == request.Talle);

    if (detalle == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No se encontró el producto en el carrito."
        });
    }

    detalle.FormaPago = request.FormaPago;

    _context.SaveChanges();

    return Json(new
    {
        exito = true,
        mensaje = "Forma de pago actualizada correctamente."
    });
}

[HttpPost]
public IActionResult EliminarDelCarrito(
    int productoId,
    string color,
    string talle)
{
    var clienteId = HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new { exito = false, mensaje = "No hay un cliente activo." });
    }

    var carrito = _context.Carritos.FirstOrDefault(c =>
        c.ClienteId == clienteId.Value &&
        c.Estado == "Activo");

    if (carrito == null)
    {
        return Json(new { exito = false, mensaje = "No hay un carrito activo." });
    }

    var detalle = _context.CarritoDetalles.FirstOrDefault(d =>
        d.CarritoId == carrito.Id &&
        d.ProductoId == productoId &&
        d.Color == color &&
        d.Talle == talle);

    if (detalle == null)
    {
        return Json(new { exito = false, mensaje = "No se encontró el producto en el carrito." });
    }

    _context.CarritoDetalles.Remove(detalle);
    _context.SaveChanges();

    return Json(new { exito = true });
}

[HttpPost]
public IActionResult VaciarCarrito()
{
    var clienteId = HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new { exito = false, mensaje = "No hay un cliente activo." });
    }

    var carrito = _context.Carritos.FirstOrDefault(c =>
        c.ClienteId == clienteId.Value &&
        c.Estado == "Activo");

    if (carrito == null)
    {
        return Json(new { exito = true });
    }

    var detalles = _context.CarritoDetalles
        .Where(d => d.CarritoId == carrito.Id)
        .ToList();

    _context.CarritoDetalles.RemoveRange(detalles);
    _context.SaveChanges();

    return Json(new { exito = true });
}

[HttpPost]
public IActionResult ConfirmarCompra()
{
    // ==========================================
    // OBTENER CLIENTE DE LA SESIÓN
    // ==========================================

    var clienteId =
        HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    // ==========================================
    // BUSCAR CARRITO ACTIVO
    // ==========================================

    var carrito = _context.Carritos
        .FirstOrDefault(c =>
            c.ClienteId == clienteId.Value &&
            c.Estado == "Activo");

    if (carrito == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No existe un carrito activo."
        });
    }

    // ==========================================
    // BUSCAR DETALLES DEL CARRITO
    // ==========================================

    var detalles = _context.CarritoDetalles
        .Where(d => d.CarritoId == carrito.Id)
        .ToList();

    if (detalles.Count == 0)
    {
        return Json(new
        {
            exito = false,
            mensaje = "El carrito está vacío."
        });
    }

    // ==========================================
    // VERIFICAR FORMAS DE PAGO
    // ==========================================

    if (detalles.Any(d =>
        string.IsNullOrWhiteSpace(d.FormaPago)))
    {
        return Json(new
        {
            exito = false,
            mensaje =
                "Todos los productos deben tener una forma de pago."
        });
    }

    // ==========================================
    // CALCULAR TOTAL
    // ==========================================

    decimal total = detalles.Sum(d =>
        d.Precio * d.Cantidad);

    // ==========================================
    // GENERAR NÚMERO DE TICKET
    // ==========================================

    var numeroTicket =
        "MM-" +
        DateTime.Now.ToString("yyyyMMdd-HHmmss");

    // ==========================================
    // CREAR VENTA
    // ==========================================

    var venta = new Venta
    {
        ClienteId = clienteId.Value,
        NumeroTicket = numeroTicket,
        Fecha = DateTime.Now,
        Total = total
    };

    _context.Ventas.Add(venta);

    _context.SaveChanges();

    // ==========================================
    // CREAR DETALLES DE LA VENTA
    // ==========================================

    foreach (var detalleCarrito in detalles)
    {
        var producto = _context.Productos
            .FirstOrDefault(p =>
                p.Id == detalleCarrito.ProductoId);

        if (producto == null)
        {
            continue;
        }

        var ventaDetalle = new VentaDetalle
        {
            VentaId = venta.Id,

            ProductoId =
                detalleCarrito.ProductoId,

            NombreProducto =
                producto.Nombre,

                Color =
    detalleCarrito.Color,

            Talle =
                detalleCarrito.Talle,

            Cantidad =
                detalleCarrito.Cantidad,

            PrecioUnitario =
                detalleCarrito.Precio,

            Subtotal =
                detalleCarrito.Precio *
                detalleCarrito.Cantidad,

            FormaPago =
                detalleCarrito.FormaPago
        };

        _context.VentaDetalles.Add(ventaDetalle);
    }

    // ==========================================
    // GUARDAR DETALLES
    // ==========================================

    _context.SaveChanges();

    // ==========================================
    // CERRAR CARRITO
    // ==========================================

    carrito.Estado = "Finalizado";

    _context.SaveChanges();

    // ==========================================
    // RESPUESTA
    // ==========================================

    return Json(new
    {
        exito = true,
        mensaje = "Compra registrada correctamente.",
        ventaId = venta.Id,
        numeroTicket = venta.NumeroTicket,
        total = venta.Total
    });
}

[HttpPost]
public IActionResult GenerarVenta(
    [FromBody] GenerarVentaRequest request)
{
    // ==========================================
    // OBTENER CLIENTE DE LA SESIÓN
    // ==========================================

    var clienteId =
        HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    // ==========================================
    // VERIFICAR DETALLES
    // ==========================================

    if (request == null ||
        request.Detalles == null ||
        request.Detalles.Count == 0)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No hay productos para generar la venta."
        });
    }

    // ==========================================
    // CREAR VENTA
    // ==========================================

    var venta = new Venta
    {
        ClienteId = clienteId.Value,
        NumeroTicket = request.NumeroTicket,
        Fecha = DateTime.Now,
        Total = 0
    };

    _context.Ventas.Add(venta);

    decimal total = 0;

    // ==========================================
    // CREAR DETALLES DE LA VENTA
    // ==========================================

    foreach (var item in request.Detalles)
    {
        var subtotal =
            item.PrecioUnitario * item.Cantidad;

        total += subtotal;

        var detalle = new VentaDetalle
        {
            Venta = venta,

            ProductoId = item.ProductoId,

            NombreProducto = item.NombreProducto,

            Color = item.Color,

            Talle = item.Talle,

            Cantidad = item.Cantidad,

            PrecioUnitario = item.PrecioUnitario,

            Subtotal = subtotal,

            FormaPago = item.FormaPago
        };

        venta.Detalles.Add(detalle);
    }

    // ==========================================
    // GUARDAR TOTAL
    // ==========================================

    venta.Total = total;

    // ==========================================
    // GUARDAR EN MYSQL
    // ==========================================

    _context.SaveChanges();

    return Json(new
    {
        exito = true,
        mensaje = "Venta registrada correctamente.",
        ventaId = venta.Id,
        numeroTicket = venta.NumeroTicket
    });
}

public IActionResult Carrito()
{
     if (HttpContext.Session.GetString("Rol") != "Cliente")
    {
        return RedirectToAction("RegistroRedSocial");
    }

    var clienteId = HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return RedirectToAction("RegistroRedSocial");
    }

     ViewBag.Usuario = HttpContext.Session.GetString("UsuarioRed") ?? "";
    ViewBag.Red = HttpContext.Session.GetString("RedSocial") ?? "";
    ViewBag.ClienteId = clienteId;

    var carrito = _context.Carritos.FirstOrDefault(c =>
        c.ClienteId == clienteId.Value &&
        c.Estado == "Activo");

   

    // ==========================================
    // VERIFICAR CLIENTE
    // ==========================================

    if (clienteId == null)
    {
        return RedirectToAction("RegistroRedSocial");
    }

    

    // ==========================================
    // SI NO EXISTE, CREARLO
    // ==========================================

    if (carrito == null)
    {
        carrito = new Carrito
        {
            ClienteId = clienteId.Value,
            Estado = "Activo",
            FechaCreacion = DateTime.Now
        };

        _context.Carritos.Add(carrito);

        _context.SaveChanges();
    }

    // ==========================================
    // DEVOLVER CARRITO
    // ==========================================

    ViewBag.CarritoId = carrito.Id;

    return View();
}

[HttpGet]
public IActionResult ObtenerCarrito()
{
    var clienteId = HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Unauthorized();
    }

    var carrito = _context.Carritos.FirstOrDefault(c =>
        c.ClienteId == clienteId.Value &&
        c.Estado == "Activo");

    if (carrito == null)
    {
        return Json(new object[] { });
    }

    var productos = _context.CarritoDetalles
        .Where(d => d.CarritoId == carrito.Id)
        .Select(d => new
        {
            id = d.ProductoId,
            nombre = d.Producto.Nombre,
            color = d.Color,
            talle = d.Talle,
            cantidad = d.Cantidad,
            precio = d.Precio,
            formaPago = d.FormaPago
        })
        .ToList();

    return Json(productos);
}

[HttpPost]
public IActionResult RegistrarVenta(
    [FromBody] RegistrarVentaRequest request)
{
    // ==========================================
    // OBTENER CLIENTE DE LA SESIÓN
    // ==========================================

    var clienteId =
        HttpContext.Session.GetInt32("ClienteId");

    if (clienteId == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    // ==========================================
    // VALIDAR DETALLES
    // ==========================================

    if (request.Detalles == null ||
        request.Detalles.Count == 0)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No hay productos para registrar la venta."
        });
    }

    // ==========================================
    // CREAR VENTA
    // ==========================================

    var venta = new Venta
    {
        ClienteId = clienteId.Value,
        NumeroTicket = request.NumeroTicket,
        Fecha = DateTime.Now,
        Total = request.Total
    };

    _context.Ventas.Add(venta);

    _context.SaveChanges();

    // ==========================================
    // CREAR DETALLES DE LA VENTA
    // ==========================================

    foreach (var item in request.Detalles)
    {
        var detalle = new VentaDetalle
        {
            VentaId = venta.Id,
            ProductoId = item.ProductoId,
            NombreProducto = item.NombreProducto,
            Color = item.Color,
            Talle = item.Talle,
            Cantidad = item.Cantidad,
            PrecioUnitario = item.PrecioUnitario,
            Subtotal = item.Subtotal,
            FormaPago = item.FormaPago
        };

        _context.VentaDetalles.Add(detalle);
    }

    // ==========================================
    // GUARDAR TODO EN MYSQL
    // ==========================================

    _context.SaveChanges();

    // ==========================================
    // CAMBIAR ESTADO DEL CARRITO
    // ==========================================

    var carrito = _context.Carritos
        .FirstOrDefault(c =>
            c.ClienteId == clienteId.Value &&
            c.Estado == "Activo");

    if (carrito != null)
    {
        carrito.Estado = "Finalizado";

        _context.SaveChanges();
    }

    return Json(new
    {
        exito = true,
        mensaje = "Venta registrada correctamente.",
        ventaId = venta.Id,
        numeroTicket = venta.NumeroTicket
    });
}

// ==========================================
// AGREGAR / QUITAR FAVORITO
// ==========================================

[HttpPost]
public IActionResult ToggleFavorito([FromBody] ToggleFavoritoRequest request)
{
    var usuarioRed =
        HttpContext.Session.GetString("UsuarioRed");

    if (string.IsNullOrWhiteSpace(usuarioRed))
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    var favorito = _context.Favoritos
        .FirstOrDefault(f =>
            f.UsuarioRed == usuarioRed &&
            f.ProductoId == request.ProductoId);

    // ==========================================
    // SI YA ES FAVORITO → QUITAR
    // ==========================================

    if (favorito != null)
    {
        _context.Favoritos.Remove(favorito);
        _context.SaveChanges();

        return Json(new
        {
            exito = true,
            favorito = false,
            mensaje = "Producto eliminado de favoritos."
        });
    }

    // ==========================================
    // SI NO ES FAVORITO → AGREGAR
    // ==========================================

    favorito = new Favorito
    {
        UsuarioRed = usuarioRed,
        ProductoId = request.ProductoId,
        FechaAgregado = DateTime.Now
    };

    _context.Favoritos.Add(favorito);
    _context.SaveChanges();

    return Json(new
    {
        exito = true,
        favorito = true,
        mensaje = "Producto agregado a favoritos."
    });
}


// ==========================================
// OBTENER FAVORITOS DEL CLIENTE
// ==========================================

[HttpGet]
public IActionResult ObtenerFavoritos()
{
    var usuarioRed =
        HttpContext.Session.GetString("UsuarioRed");

    if (string.IsNullOrWhiteSpace(usuarioRed))
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    var favoritos = _context.Favoritos
        .Where(f => f.UsuarioRed == usuarioRed)
        .Select(f => new
        {
            id = f.ProductoId,
            nombre = f.Producto.Nombre,
            precio = f.Producto.Precio,
            imagen = f.Producto.Imagen
        })
        .ToList();

    return Json(new
    {
        exito = true,
        favoritos = favoritos
    });
}


[HttpPost]
public IActionResult AgregarValoracion(
    [FromBody] AgregarValoracionRequest request)
{
    var usuarioRed = HttpContext.Session.GetString("UsuarioRed");

    if (string.IsNullOrWhiteSpace(usuarioRed))
    {
        return Json(new
        {
            exito = false,
            mensaje = "La sesión del cliente no está disponible."
        });
    }

    // ==========================================
    // VALIDAR PUNTUACIÓN
    // ==========================================

    if (request.Puntuacion < 1 || request.Puntuacion > 5)
    {
        return Json(new
        {
            exito = false,
            mensaje = "La puntuación debe estar entre 1 y 5 estrellas."
        });
    }

    // ==========================================
    // BUSCAR CLIENTE
    // ==========================================

    var cliente = _context.Clientes
        .FirstOrDefault(c =>
            c.UsuarioRed == usuarioRed);

    if (cliente == null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "No se encontró el cliente."
        });
    }

    // ==========================================
    // VERIFICAR SI YA VALORÓ EL PRODUCTO
    // ==========================================

    var valoracionExistente = _context.Valoraciones
        .FirstOrDefault(v =>
            v.ProductoId == request.ProductoId &&
            v.ClienteId == cliente.Id);

    if (valoracionExistente != null)
    {
        return Json(new
        {
            exito = false,
            mensaje = "Ya valoraste este producto."
        });
    }

    // ==========================================
    // CREAR VALORACIÓN
    // ==========================================

    var valoracion = new Valoracion
    {
        ProductoId = request.ProductoId,
        ClienteId = cliente.Id,
        Puntuacion = request.Puntuacion,
        Comentario = request.Comentario,
        Fecha = DateTime.Now
    };

    _context.Valoraciones.Add(valoracion);
    _context.SaveChanges();

    return Json(new
    {
        exito = true,
        mensaje = "¡Gracias por tu valoración! ⭐",
        puntuacion = valoracion.Puntuacion
    });
}

[HttpGet]
public IActionResult ObtenerValoracionProducto(int productoId)
{
    var resultado = _context.Valoraciones
        .Where(v => v.ProductoId == productoId)
        .GroupBy(v => v.ProductoId)
        .Select(g => new
        {
            productoId = g.Key,
            promedio = g.Average(v => v.Puntuacion),
            cantidad = g.Count()
        })
        .FirstOrDefault();

    if (resultado == null)
    {
        return Json(new
        {
            exito = true,
            productoId = productoId,
            promedio = 0,
            cantidad = 0
        });
    }

    return Json(new
    {
        exito = true,
        productoId = resultado.productoId,
        promedio = Math.Round(resultado.promedio, 1),
        cantidad = resultado.cantidad
    });
}

[HttpGet]
public IActionResult ObtenerValoracionesProductos()
{
    var valoraciones = _context.Valoraciones
        .GroupBy(v => v.ProductoId)
        .Select(g => new
        {
            productoId = g.Key,
            promedio = Math.Round(g.Average(v => v.Puntuacion), 1),
            cantidad = g.Count()
        })
        .ToList();

    return Json(new
    {
        exito = true,
        valoraciones = valoraciones
    });
}

}// ← termina HomeController


public class ToggleFavoritoRequest
{
    public int ProductoId { get; set; }
}

public class AgregarValoracionRequest
{
    public int ProductoId { get; set; }

    public int Puntuacion { get; set; }

    public string? Comentario { get; set; }
}