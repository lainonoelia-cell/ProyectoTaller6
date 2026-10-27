namespace ProyectoTaller6.Models
{
    public class ProductoVariante
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public string Color { get; set; } = "";

        public string Talle { get; set; } = "";

        public int Stock { get; set; }
    }
}