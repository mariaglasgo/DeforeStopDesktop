using Microsoft.EntityFrameworkCore;

namespace DeforeStopDesktop.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Zona> Zonas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<ImagenSatelital> ImagenesSatelitales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer("Server=DESKTOP-SUAKISQ;Database=DeforeStopMVC;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Zona>().ToTable("Zonas");
            modelBuilder.Entity<Reporte>().ToTable("Reportes");
            modelBuilder.Entity<ImagenSatelital>().ToTable("ImagenesSatelitales");
        }
    }
}