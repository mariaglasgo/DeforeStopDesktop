using System;
using System.Collections.Generic;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class AlertaController
    {
        public List<Alerta> ObtenerTodas()
        {
            using (var db = new AppDbContext())
            {
                var alertas = db.Alertas.OrderByDescending(a => a.FechaAlerta).ToList();

                // Cargar el nombre de la zona
                foreach (var alerta in alertas)
                {
                    var zona = db.Zonas.Find(alerta.ZonaId);
                    if (zona != null)
                        alerta.NombreZona = zona.Nombre;
                }

                return alertas;
            }
        }

        public Alerta? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                var alerta = db.Alertas.Find(id);
                if (alerta != null)
                {
                    var zona = db.Zonas.Find(alerta.ZonaId);
                    if (zona != null)
                        alerta.NombreZona = zona.Nombre;
                }
                return alerta;
            }
        }

        public bool Crear(Alerta alerta)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    alerta.FechaAlerta = DateTime.Now;
                    db.Alertas.Add(alerta);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(Alerta alerta)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var a = db.Alertas.Find(alerta.Id);
                    if (a == null) return false;

                    a.Mensaje = alerta.Mensaje;
                    a.ZonaId = alerta.ZonaId;
                    a.Leida = alerta.Leida;

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
                    var alerta = db.Alertas.Find(id);
                    if (alerta == null) return false;

                    db.Alertas.Remove(alerta);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool MarcarComoLeida(int id)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var alerta = db.Alertas.Find(id);
                    if (alerta == null) return false;

                    alerta.Leida = true;
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public int ContarNoLeidas()
        {
            using (var db = new AppDbContext())
            {
                return db.Alertas.Count(a => !a.Leida);
            }
        }
    }
}