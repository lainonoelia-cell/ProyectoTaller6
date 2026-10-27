namespace ProyectoTaller6.Models
{
    public class CarritoDetalle
    {
        public int Id { get; set; }

        public int CarritoId { get; set; }

        public Carrito Carrito { get; set; } = null!;

        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        public string Color { get; set; } = "";

        public string Talle { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public string FormaPago { get; set; } = "";
    }
}