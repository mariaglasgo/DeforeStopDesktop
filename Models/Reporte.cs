using System;
using System.ComponentModel.DataAnnotations;

namespace DeforeStopDesktop.Models
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int ZonaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    }
}