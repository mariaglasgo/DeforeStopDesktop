using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeforeStopDesktop.Models
{
    public class Alerta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; } = string.Empty;

        public int ZonaId { get; set; }

        public DateTime FechaAlerta { get; set; } = DateTime.Now;

        public bool Leida { get; set; } = false;

        // Propiedad para mostrar el nombre de la zona (NO se guarda en BD)
        [NotMapped]
        public string? NombreZona { get; set; }
    }
}