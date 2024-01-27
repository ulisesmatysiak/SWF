using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SWF_API.Models;

public partial class SWFDBContext : DbContext
{
    public SWFDBContext()
    {
    }

    public SWFDBContext(DbContextOptions<SWFDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Campeonato> Campeonatos { get; set; }

    public virtual DbSet<Fecha> Fechas { get; set; }

    public virtual DbSet<Jugador> Jugadores { get; set; }

    public virtual DbSet<Tweet> Tweets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campeonato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CAMPEONA__3214EC27ECA96679");

            entity.ToTable("CAMPEONATOS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
        });

        modelBuilder.Entity<Fecha>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FECHA__3214EC27D2B2A7C8");

            entity.ToTable("FECHA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JUGADORES__3214EC275B6ACBF1");

            entity.ToTable("JUGADORES");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Camiseta).HasColumnName("CAMISETA");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DESCRIPCION");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGEN_URL");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NOMBRE");
            entity.Property(e => e.IdCampeonato).HasColumnName("IdCampeonato");
            entity.HasOne(d => d.IdCampeonatoNavigation).WithMany(p => p.Jugadores)
                .HasForeignKey(d => d.IdCampeonato)
                .HasConstraintName("FK__JUGADORES__CAMPEONATOS");
        });

        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TWEETS__3214EC27F5F57AF8");

            entity.ToTable("TWEETS");

            entity.Property(e => e.Id).HasColumnName("ID");
            //entity.Property(e => e.IdCampeonato).HasColumnName("ID_CAMPEONATO");
            entity.Property(e => e.IdFecha).HasColumnName("ID_FECHA");
            entity.Property(e => e.IdJugador).HasColumnName("ID_JUGADOR");

            //entity.HasOne(d => d.IdCampeonatoNavigation).WithMany(p => p.Tweets)
            //    .HasForeignKey(d => d.IdCampeonato)
            //    .HasConstraintName("FK__TWEETS__ID_CAMPE__5070F446");

            entity.HasOne(d => d.IdFechaNavigation).WithMany(p => p.Tweets)
                .HasForeignKey(d => d.IdFecha)
                .HasConstraintName("FK__TWEETS__ID_FECHA__5165187F");

            entity.HasOne(d => d.oJugador).WithMany(p => p.Tweets)
                .HasForeignKey(d => d.IdJugador)
                .HasConstraintName("FK__TWEETS__ID_JUGAD__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
