namespace ProyectoTaller6.Models
{
    public class RegistrarVentaRequest
    {
        public string NumeroTicket { get; set; } = "";

        public decimal Total { get; set; }

        public List<RegistrarVentaDetalleRequest> Detalles { get; set; }
            = new List<RegistrarVentaDetalleRequest>();
    }

    public class RegistrarVentaDetalleRequest
    {
        public int ProductoId { get; set; }

        public string NombreProducto { get; set; } = "";

        public string Color { get; set; } = "";

        public string Talle { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }

        public string FormaPago { get; set; } = "";
    }
}