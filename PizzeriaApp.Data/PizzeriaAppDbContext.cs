using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Data.Entities;

namespace PizzeriaApp.Data;

public class PizzeriaAppDbContext : DbContext
{
    public PizzeriaAppDbContext(DbContextOptions<PizzeriaAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<TokenRecuperacion> TokensRecuperacion => Set<TokenRecuperacion>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Estado>().ToTable("Estados");
        modelBuilder.Entity<Rol>().ToTable("Roles");
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
        modelBuilder.Entity<TokenRecuperacion>().ToTable("TokensRecuperacion");
        modelBuilder.Entity<Categoria>().ToTable("Categorias");
        modelBuilder.Entity<Producto>().ToTable("Productos");

        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(entity => entity.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estado>().HasData(
            new Estado { EstadoId = 1, Nombre = "Activo", Descripcion = "Registro activo" },
            new Estado { EstadoId = 2, Nombre = "Inactivo", Descripcion = "Registro inactivo" },
            new Estado { EstadoId = 3, Nombre = "Suspendido", Descripcion = "Registro suspendido" }
        );

        modelBuilder.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "Administrador", Descripcion = "Control total del sistema" },
            new Rol { RolId = 2, Nombre = "Usuario", Descripcion = "Cliente de la pizzería" }
        );
    }
}