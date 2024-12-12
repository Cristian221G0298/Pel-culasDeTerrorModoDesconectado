using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace PelículasDeTerrorModoDesconectado.Models;

public partial class PelículasDeTerrorContext : DbContext
{
    public PelículasDeTerrorContext()
    {
    }

    public PelículasDeTerrorContext(DbContextOptions<PelículasDeTerrorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Pelicula> Pelicula { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;password=root;user=root;database=PelículasDeTerror;port=3307", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.3.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Director).HasMaxLength(60);
            entity.Property(e => e.Duracion).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Portada).HasColumnType("text");
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.Sinopsis).HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
