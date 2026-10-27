namespace ProyectoTaller6.Models
{
    public class GenerarVentaRequest
    {
        public string NumeroTicket { get; set; } = "";

        public List<GenerarVentaDetalleRequest> Detalles { get; set; }
            = new List<GenerarVentaDetalleRequest>();
    }

    public class GenerarVentaDetalleRequest
    {
        public int ProductoId { get; set; }

        public string NombreProducto { get; set; } = "";

        public string Color { get; set; } = "";

        public string Talle { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public string FormaPago { get; set; } = "";
    }
}