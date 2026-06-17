using Microsoft.EntityFrameworkCore;
using EcoRaeeUac.Models;

namespace EcoRaeeUac.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CampañaAmbiental> Campañas { get; set; }
    public DbSet<RecoleccionRaee> Recolecciones { get; set; }
    public DbSet<Participante> Participantes { get; set; }
    public DbSet<MaterialEducativo> MaterialesEducativos { get; set; }
    public DbSet<Docente> Docentes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Docente>()
            .HasIndex(d => d.Email)
            .IsUnique();

        modelBuilder.Entity<CampañaAmbiental>()
            .HasOne(c => c.Responsable)
            .WithMany()
            .HasForeignKey(c => c.ResponsableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Participante>()
            .HasOne(p => p.Docente)
            .WithMany()
            .HasForeignKey(p => p.DocenteId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
