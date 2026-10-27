using System;

namespace ProyectoTaller6.Models
{
    public class Favorito
    {
        public int Id { get; set; }

        public string UsuarioRed { get; set; }

        public int ProductoId { get; set; }

        public DateTime FechaAgregado { get; set; }

        public Producto Producto { get; set; }
    }
}