using System;
using System.Collections.Generic;
using System.Linq;
using DeforeStopDesktop.Models;

namespace DeforeStopDesktop.Controllers
{
    public class UsuarioController
    {
        public List<Usuario> ObtenerTodos()
        {
            using (var db = new AppDbContext())
            {
                return db.Usuarios.OrderBy(u => u.Id).ToList();
            }
        }

        public Usuario? ObtenerPorId(int id)
        {
            using (var db = new AppDbContext())
            {
                return db.Usuarios.Find(id);
            }
        }

        public bool Crear(Usuario usuario)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    usuario.FechaCreacion = DateTime.Now;
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public bool Actualizar(Usuario usuario)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var u = db.Usuarios.Find(usuario.Id);
                    if (u == null) return false;

                    u.Nombre = usuario.Nombre;
                    u.Correo = usuario.Correo;
                    u.Contrasena = usuario.Contrasena;
                    u.Rol = usuario.Rol;
                    u.Activo = usuario.Activo;

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
                    var usuario = db.Usuarios.Find(id);
                    if (usuario == null) return false;

                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    return true;
                }
            }
            catch { return false; }
        }

        public Usuario? ValidarLogin(string correo, string contrasena)
        {
            using (var db = new AppDbContext())
            {
                return db.Usuarios.FirstOrDefault(u =>
                    u.Correo == correo && u.Contrasena == contrasena && u.Activo);
            }
        }
    }
}