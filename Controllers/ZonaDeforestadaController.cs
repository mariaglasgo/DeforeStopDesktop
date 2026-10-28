using System;
using System.Collections.Generic;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class ZonaDeforestadaController
    {
        public List<ZonaDeforestada> ObtenerTodas()
        {
            using (var db = new AppDbContext())
            {
                var zonas = db.ZonasDeforestadas.OrderByDescending(z => z.FechaDeteccion).ToList();

                foreach (var zona in zonas)
                {
                    var z = db.Zonas.Find(zona.ZonaId);
                    if (z != null)
                        zona.NombreZona = z.Nombre;
                }

                return zonas;
            }
        }

        public ZonaDeforestada? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                var zona = db.ZonasDeforestadas.Find(id);
                if (zona != null)
                {
                    var z = db.Zonas.Find(zona.ZonaId);
                    if (z != null)
                        zona.NombreZona = z.Nombre;
                }
                return zona;
            }
        }

        public bool Crear(ZonaDeforestada zona)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.ZonasDeforestadas.Add(zona);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(ZonaDeforestada zona)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var z = db.ZonasDeforestadas.Find(zona.Id);
                    if (z == null) return false;

                    z.ZonaId = zona.ZonaId;
                    z.Hectareas = zona.Hectareas;
                    z.FechaDeteccion = zona.FechaDeteccion;
                    z.NivelAlerta = zona.NivelAlerta;

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
                    var zona = db.ZonasDeforestadas.Find(id);
                    if (zona == null) return false;

                    db.ZonasDeforestadas.Remove(zona);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}