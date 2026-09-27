using System;
using System.Collections.Generic;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class ReporteController
    {
        public List<Reporte> ObtenerTodos()
        {
            using (var db = new AppDbContext())
            {
                return db.Reportes.OrderBy(r => r.Id).ToList();
            }
        }

        public Reporte? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Reportes.Find(id);
            }
        }

        public bool Crear(Reporte reporte)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    reporte.FechaGeneracion = DateTime.Now;
                    db.Reportes.Add(reporte);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(Reporte reporte)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var r = db.Reportes.Find(reporte.Id);
                    if (r == null) return false;

                    r.Titulo = reporte.Titulo;
                    r.FechaInicio = reporte.FechaInicio;
                    r.FechaFin = reporte.FechaFin;
                    r.ZonaId = reporte.ZonaId;
                    r.UsuarioId = reporte.UsuarioId;

                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var reporte = db.Reportes.Find(id);
                    if (reporte == null) return false;

                    db.Reportes.Remove(reporte);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}