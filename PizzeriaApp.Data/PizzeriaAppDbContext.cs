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
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<Tamano> Tamanos => Set<Tamano>();
    public DbSet<TipoMasa> TiposMasa => Set<TipoMasa>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<DetallePedido> DetallesPedido => Set<DetallePedido>();
    public DbSet<DetallePedidoIngrediente> DetallePedidoIngredientes => Set<DetallePedidoIngrediente>();

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
        modelBuilder.Entity<Ingrediente>().ToTable("Ingredientes");
        modelBuilder.Entity<Tamano>().ToTable("Tamanos");
        modelBuilder.Entity<TipoMasa>().ToTable("TiposMasa");
        modelBuilder.Entity<Pedido>().ToTable("Pedidos");
        modelBuilder.Entity<DetallePedido>().ToTable("DetallesPedido");
        modelBuilder.Entity<DetallePedidoIngrediente>().ToTable("DetallePedidoIngredientes");

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