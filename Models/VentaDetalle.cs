namespace ProyectoTaller6.Models
{
    public class VentaDetalle
    {
        public int Id { get; set; }

        public int VentaId { get; set; }

        public Venta Venta { get; set; } = null!;

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