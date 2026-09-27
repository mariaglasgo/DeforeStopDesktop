using System.Collections.Generic;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class ZonaController
    {
        public List<Zona> ObtenerTodas()
        {
            using (var db = new AppDbContext())
            {
                return db.Zonas.OrderBy(z => z.Id).ToList();
            }
        }

        public Zona? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Zonas.Find(id);
            }
        }

        public bool Crear(Zona zona)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    db.Zonas.Add(zona);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(Zona zona)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var z = db.Zonas.Find(zona.Id);
                    if (z == null) return false;

                    z.Nombre = zona.Nombre;
                    z.Departamento = zona.Departamento;
                    z.Latitud = zona.Latitud;
                    z.Longitud = zona.Longitud;

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
                    var zona = db.Zonas.Find(id);
                    if (zona == null) return false;

                    db.Zonas.Remove(zona);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}