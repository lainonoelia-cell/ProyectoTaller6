using Microsoft.EntityFrameworkCore;

namespace ProyectoTaller6.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Subcategoria> Subcategorias { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Carrito> Carritos { get; set; }

        public DbSet<CarritoDetalle> CarritoDetalles { get; set; }
        
        public DbSet<ProductoVariante> ProductoVariantes { get; set; }

        public DbSet<Venta> Ventas { get; set; }

        public DbSet<VentaDetalle> VentaDetalles { get; set; }

        public DbSet<Favorito> Favoritos { get; set; }

        public DbSet<Valoracion> Valoraciones { get; set; }

        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // =========================================
    // CLIENTE ÚNICO POR USUARIO + RED SOCIAL
    // =========================================

    modelBuilder.Entity<Cliente>()
        .HasIndex(c => new { c.UsuarioRed, c.RedSocial })
        .IsUnique();

    // =========================================
    // CLIENTE → CARRITOS
    // =========================================

    modelBuilder.Entity<Carrito>()
        .HasOne(c => c.Cliente)
        .WithMany(c => c.Carritos)
        .HasForeignKey(c => c.ClienteId)
        .OnDelete(DeleteBehavior.Cascade);

    // =========================================
    // CARRITO → DETALLES
    // =========================================

    modelBuilder.Entity<CarritoDetalle>()
        .HasOne(d => d.Carrito)
        .WithMany(c => c.Detalles)
        .HasForeignKey(d => d.CarritoId)
        .OnDelete(DeleteBehavior.Cascade);

    // =========================================
    // PRODUCTO → DETALLE CARRITO
    // =========================================

    modelBuilder.Entity<CarritoDetalle>()
        .HasOne(d => d.Producto)
        .WithMany()
        .HasForeignKey(d => d.ProductoId)
        .OnDelete(DeleteBehavior.Restrict);

    // =========================================
    // DECIMALES
    // =========================================

    modelBuilder.Entity<Producto>()
        .Property(p => p.Precio)
        .HasPrecision(18, 2);

    modelBuilder.Entity<CarritoDetalle>()
        .Property(d => d.Precio)
        .HasPrecision(18, 2);

        // =========================================
// CLIENTE → VENTAS
// =========================================

modelBuilder.Entity<Venta>()
    .HasOne(v => v.Cliente)
    .WithMany(c => c.Ventas)
    .HasForeignKey(v => v.ClienteId)
    .OnDelete(DeleteBehavior.Restrict);


// =========================================
// VENTA → DETALLES
// =========================================

modelBuilder.Entity<VentaDetalle>()
    .HasOne(d => d.Venta)
    .WithMany(v => v.Detalles)
    .HasForeignKey(d => d.VentaId)
    .OnDelete(DeleteBehavior.Cascade);


// =========================================
// DECIMALES DE VENTA
// =========================================

modelBuilder.Entity<Venta>()
    .Property(v => v.Total)
    .HasPrecision(18, 2);

modelBuilder.Entity<VentaDetalle>()
    .Property(d => d.PrecioUnitario)
    .HasPrecision(18, 2);

modelBuilder.Entity<VentaDetalle>()
    .Property(d => d.Subtotal)
    .HasPrecision(18, 2);


// =========================================
// PRODUCTO → VENTA DETALLE
// =========================================

modelBuilder.Entity<VentaDetalle>()
    .HasOne<Producto>()
    .WithMany()
    .HasForeignKey(d => d.ProductoId)
    .OnDelete(DeleteBehavior.Restrict);

    // =========================================
// FAVORITO → PRODUCTO
// =========================================

modelBuilder.Entity<Favorito>()
    .HasOne(f => f.Producto)
    .WithMany()
    .HasForeignKey(f => f.ProductoId)
    .OnDelete(DeleteBehavior.Cascade);

// =========================================
// FAVORITO ÚNICO POR USUARIO + PRODUCTO
// =========================================

modelBuilder.Entity<Favorito>()
    .HasIndex(f => new { f.UsuarioRed, f.ProductoId })
    .IsUnique();

    // =========================================
// VALORACION
// =========================================

    modelBuilder.Entity<Valoracion>()
    .HasOne(v => v.Producto)
    .WithMany()
    .HasForeignKey(v => v.ProductoId)
    .OnDelete(DeleteBehavior.Cascade);

modelBuilder.Entity<Valoracion>()
    .HasOne(v => v.Cliente)
    .WithMany()
    .HasForeignKey(v => v.ClienteId)
    .OnDelete(DeleteBehavior.Restrict);
}


    }


    
}