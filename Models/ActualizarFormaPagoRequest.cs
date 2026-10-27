namespace ProyectoTaller6.Models
{
    public class ActualizarFormaPagoRequest
    {
        public int ProductoId { get; set; }

        public string Talle { get; set; } = "";

        public string FormaPago { get; set; } = "";
    }
}