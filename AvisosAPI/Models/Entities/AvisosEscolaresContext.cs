using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AvisosAPI.Models.Entities;

public partial class AvisosEscolaresContext : DbContext
{
    public AvisosEscolaresContext()
    {
    }

    public AvisosEscolaresContext(DbContextOptions<AvisosEscolaresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Alumnoavisogeneral> Alumnoavisogeneral { get; set; }

    public virtual DbSet<Avisogeneral> Avisogeneral { get; set; }

    public virtual DbSet<Avisopersonal> Avisopersonal { get; set; }

    public virtual DbSet<Estadoaviso> Estadoaviso { get; set; }

    public virtual DbSet<Grupo> Grupo { get; set; }

    public virtual DbSet<Maestro> Maestro { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.IdGrupo, "FK_Alumno_Grupo");

            entity.HasIndex(e => e.NumControl, "UQ_Alumno_NC").IsUnique();

            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NumControl).HasMaxLength(8);

            entity.HasOne(d => d.IdGrupoNavigation).WithMany(p => p.Alumno)
                .HasForeignKey(d => d.IdGrupo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Alumno_Grupo");
        });

        modelBuilder.Entity<Alumnoavisogeneral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumnoavisogeneral");

            entity.HasIndex(e => e.IdAlumno, "FK_AlumnoAvisoGeneral_Alumno");

            entity.HasIndex(e => e.IdAvisoGeneral, "FK_AlumnoAvisoGeneral_Aviso");

            entity.HasIndex(e => e.IdEstado, "FK_AlumnoAvisoGeneral_Estado");

            entity.Property(e => e.FechaLeido).HasColumnType("datetime");
            entity.Property(e => e.IdEstado).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Alumnoavisogeneral)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlumnoAvisoGeneral_Alumno");

            entity.HasOne(d => d.IdAvisoGeneralNavigation).WithMany(p => p.Alumnoavisogeneral)
                .HasForeignKey(d => d.IdAvisoGeneral)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlumnoAvisoGeneral_Aviso");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Alumnoavisogeneral)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlumnoAvisoGeneral_Estado");
        });

        modelBuilder.Entity<Avisogeneral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avisogeneral");

            entity.HasIndex(e => e.IdMaestro, "FK_AvisoGeneral_Maestro");

            entity.Property(e => e.Contenido).HasMaxLength(250);
            entity.Property(e => e.FechaEnviado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaExpira).HasColumnType("datetime");
            entity.Property(e => e.Titulo).HasMaxLength(50);

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Avisogeneral)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvisoGeneral_Maestro");
        });

        modelBuilder.Entity<Avisopersonal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avisopersonal");

            entity.HasIndex(e => e.IdAlumno, "FK_AvisoPersonal_Alumno");

            entity.HasIndex(e => e.IdEstado, "FK_AvisoPersonal_Estado");

            entity.HasIndex(e => e.IdMaestro, "FK_AvisoPersonal_Maestro");

            entity.Property(e => e.Contenido).HasMaxLength(250);
            entity.Property(e => e.FechaEnviado)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaLeido).HasColumnType("datetime");
            entity.Property(e => e.IdEstado).HasDefaultValueSql("'1'");
            entity.Property(e => e.Titulo).HasMaxLength(50);

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvisoPersonal_Alumno");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.IdEstado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvisoPersonal_Estado");

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvisoPersonal_Maestro");
        });

        modelBuilder.Entity<Estadoaviso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estadoaviso");

            entity.Property(e => e.Nombre).HasMaxLength(20);
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("grupo");

            entity.HasIndex(e => e.IdMaestro, "FK_Grupo_Maestro");

            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.IdMaestroNavigation).WithMany(p => p.Grupo)
                .HasForeignKey(d => d.IdMaestro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grupo_Maestro");
        });

        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("maestro");

            entity.HasIndex(e => e.NumControl, "UQ_Maestro_NC").IsUnique();

            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NumControl).HasMaxLength(4);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
