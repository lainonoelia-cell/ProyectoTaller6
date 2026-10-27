namespace ProyectoTaller6.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public string NumeroTicket { get; set; } = "";

        public DateTime Fecha { get; set; } = DateTime.Now;

        public decimal Total { get; set; }

        public ICollection<VentaDetalle> Detalles { get; set; }
            = new List<VentaDetalle>();
    }
}