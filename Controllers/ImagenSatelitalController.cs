using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class ImagenSatelitalController
    {
        public List<ImagenSatelital> ObtenerTodas()
        {
            using (var db = new AppDbContext())
            {
                return db.ImagenesSatelitales.OrderBy(i => i.Id).ToList();
            }
        }

        public ImagenSatelital? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.ImagenesSatelitales.Find(id);
            }
        }

        public bool Crear(ImagenSatelital imagen)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    imagen.FechaSubida = DateTime.Now;
                    db.ImagenesSatelitales.Add(imagen);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(ImagenSatelital imagen)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var i = db.ImagenesSatelitales.Find(imagen.Id);
                    if (i == null) return false;

                    i.NombreArchivo = imagen.NombreArchivo;
                    i.RutaArchivo = imagen.RutaArchivo;
                    i.FechaCaptura = imagen.FechaCaptura;
                    i.ZonaId = imagen.ZonaId;
                    i.UsuarioId = imagen.UsuarioId;

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
                    var imagen = db.ImagenesSatelitales.Find(id);
                    if (imagen == null) return false;

                    // Borrar archivo físico si existe
                    if (!string.IsNullOrEmpty(imagen.RutaArchivo) &&
                        !imagen.RutaArchivo.StartsWith("http") &&
                        File.Exists(imagen.RutaArchivo))
                    {
                        try { File.Delete(imagen.RutaArchivo); } catch { }
                    }

                    db.ImagenesSatelitales.Remove(imagen);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public string CopiarArchivoALocal(string rutaOrigen)
        {
            try
            {
                string carpetaUploads = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");
                if (!Directory.Exists(carpetaUploads))
                    Directory.CreateDirectory(carpetaUploads);

                if (rutaOrigen.StartsWith(carpetaUploads))
                    return rutaOrigen;

                string extension = Path.GetExtension(rutaOrigen);
                string nombreUnico = Guid.NewGuid().ToString() + extension;
                string rutaDestino = Path.Combine(carpetaUploads, nombreUnico);

                File.Copy(rutaOrigen, rutaDestino, true);
                return rutaDestino;
            }
            catch { return rutaOrigen; }
        }
    }
}