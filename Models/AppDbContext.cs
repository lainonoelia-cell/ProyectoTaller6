using Microsoft.EntityFrameworkCore;

namespace ProyectoTaller6.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

       public DbSet<Categoria> Categorias { get; set; }
    

    public DbSet<Subcategoria> Subcategorias { get; set; }

        }

    }
