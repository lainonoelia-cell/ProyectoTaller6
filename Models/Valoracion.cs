using System;

namespace ProyectoTaller6.Models
{
    public class Valoracion
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public int ClienteId { get; set; }

        public int Puntuacion { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha { get; set; }

        // Relaciones

        public Producto Producto { get; set; }

        public Cliente Cliente { get; set; }
    }
}