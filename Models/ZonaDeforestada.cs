using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeforeStopDesktop.Models
{
    public class ZonaDeforestada
    {
        [Key]
        public int Id { get; set; }

        public int ZonaId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Hectareas { get; set; }

        public DateTime FechaDeteccion { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string NivelAlerta { get; set; } = "Bajo";

        [NotMapped]
        public string? NombreZona { get; set; }
    }
}