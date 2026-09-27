using System.ComponentModel.DataAnnotations;

namespace DeforeStopDesktop.Models
{
    public class Zona
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Departamento { get; set; } = string.Empty;
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
    }
}