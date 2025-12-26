using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Data;

/// <summary>
/// DbContext principal de la aplicación
/// Define las tablas de base de datos y sus relaciones
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Constructor que recibe opciones de configuración
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// DbSet de usuarios
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// DbSet de tickets
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; } = null!;

    /// <summary>
    /// Configuración del modelo (relaciones, restricciones, etc.)
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la tabla Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configuración de la tabla Tickets
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Priority).HasConversion<int>();

            // Relación: Ticket fue creado por un User (CreatedByUser)
            entity.HasOne(e => e.CreatedByUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ticket_CreatedByUser");

            // Relación: Ticket está asignado a un User (AssignedToUser)
            entity.HasOne(e => e.AssignedToUser)
                .WithMany()
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Ticket_AssignedToUser");
        });

        // Seed inicial: Usuario admin
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Datos iniciales de la base de datos
    /// </summary>
    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Usuario admin preconfigurado
        // En producción, la contraseña debe estar hasheada con bcrypt o similar
        var adminUser = new User
        {
            Id = 1,
            Username = "admin",
            Password = "123456", // Nota: En producción debería estar hasheada
            FullName = "Administrador del Sistema",
            Email = "admin@helpdesk.local",
            IsActive = true,
            CreatedAt = new DateTime(2025, 12, 26, 0, 0, 0, DateTimeKind.Utc)
        };

        modelBuilder.Entity<User>().HasData(adminUser);
    }
}
