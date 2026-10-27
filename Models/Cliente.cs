using System.ComponentModel.DataAnnotations;

namespace ProyectoTaller6.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioRed { get; set; } = "";

        [Required]
        public string RedSocial { get; set; } = "";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public ICollection<Carrito> Carritos { get; set; }
            = new List<Carrito>();

        public ICollection<Venta> Ventas { get; set; }
            = new List<Venta>();
    }
}