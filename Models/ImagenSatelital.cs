using System;
using System.ComponentModel.DataAnnotations;

namespace DeforeStopDesktop.Models
{
    public class ImagenSatelital
    {
        [Key]
        public int Id { get; set; }
        public string NombreArchivo { get; set; } = string.Empty;
        public string? RutaArchivo { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int ZonaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}