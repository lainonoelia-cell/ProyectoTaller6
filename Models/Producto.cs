namespace ProyectoTaller6.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int Precio { get; set; }

        public string Categoria { get; set; } // 👈 ESTA FALTABA
        public string Tipo { get; set; }      // 👈 para filtros

        public string Imagen { get; set; }

        public string Codigo { get; set; }
    }
}