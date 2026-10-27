namespace ProyectoTaller6.Models
{
    public class AgregarCarritoRequest
    {
        public int ProductoId { get; set; }

          public string Color { get; set; } = "";

        public string Talle { get; set; } = "";

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

       public string FormaPago { get; set; } = "";
    }
}