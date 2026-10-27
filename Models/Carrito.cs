namespace ProyectoTaller6.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public string Estado { get; set; } = "Activo";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<CarritoDetalle> Detalles { get; set; }
            = new List<CarritoDetalle>();
    }
}