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
    public DbSet<Insumo> Insumos => Set<Insumo>();
    public DbSet<MovimientoInventario> MovimientosInventario => Set<MovimientoInventario>();
    public DbSet<RecetaIngrediente> RecetaIngredientes => Set<RecetaIngrediente>();
    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<ConfiguracionReserva> ConfiguracionesReserva => Set<ConfiguracionReserva>();
    public DbSet<Cupon> Cupones => Set<Cupon>();
    public DbSet<PromocionPersonalizada> PromocionesPersonalizadas => Set<PromocionPersonalizada>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Direccion> Direcciones => Set<Direccion>();
    public DbSet<OrdenCompra> OrdenesCompra => Set<OrdenCompra>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<DetalleOrdenCompra> DetallesOrdenCompra => Set<DetalleOrdenCompra>();
    public DbSet<Evaluacion> Evaluaciones => Set<Evaluacion>();
    public DbSet<PuntoRecompensa> PuntosRecompensa => Set<PuntoRecompensa>();

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
        modelBuilder.Entity<Insumo>().ToTable("Insumos");
        modelBuilder.Entity<MovimientoInventario>().ToTable("MovimientosInventario");
        modelBuilder.Entity<RecetaIngrediente>().ToTable("RecetaIngredientes");
        modelBuilder.Entity<Reserva>().ToTable("Reservas");
        modelBuilder.Entity<ConfiguracionReserva>().ToTable("ConfiguracionesReserva");
        modelBuilder.Entity<Cupon>().ToTable("Cupones");
        modelBuilder.Entity<Evento>().ToTable("Eventos");
        modelBuilder.Entity<Direccion>().ToTable("Direcciones");
        modelBuilder.Entity<OrdenCompra>().ToTable("OrdenesCompra");
        modelBuilder.Entity<Proveedor>().ToTable("Proveedores");
        modelBuilder.Entity<DetalleOrdenCompra>().ToTable("DetallesOrdenCompra");
        modelBuilder.Entity<Evaluacion>().ToTable("Evaluaciones");
        modelBuilder.Entity<PuntoRecompensa>().ToTable("PuntosRecompensa");


        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(entity => entity.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // ==========================================
        // PRECISIÓN DE PROPIEDADES DECIMALES
        // ==========================================

        // DINERO: 18 dígitos, 2 decimales
        modelBuilder.Entity<DetalleOrdenCompra>().Property(x => x.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<DetallePedido>().Property(x => x.PrecioUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<DetallePedidoIngrediente>().Property(x => x.PrecioExtra).HasPrecision(18, 2);
        modelBuilder.Entity<Ingrediente>().Property(x => x.PrecioExtra).HasPrecision(18, 2);
        modelBuilder.Entity<Insumo>().Property(x => x.CostoUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<MovimientoInventario>().Property(x => x.CostoUnitario).HasPrecision(18, 2);
        modelBuilder.Entity<OrdenCompra>().Property(x => x.Total).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(x => x.Subtotal).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(x => x.Impuestos).HasPrecision(18, 2);
        modelBuilder.Entity<Pedido>().Property(x => x.Total).HasPrecision(18, 2);
        modelBuilder.Entity<Producto>().Property(x => x.Precio).HasPrecision(18, 2);
        modelBuilder.Entity<PromocionPersonalizada>().Property(x => x.MontoDescontado).HasPrecision(18, 2);
        modelBuilder.Entity<TipoMasa>().Property(x => x.PrecioExtra).HasPrecision(18, 2);

        // PORCENTAJES: 5 dígitos, 2 decimales
        modelBuilder.Entity<Cupon>().Property(x => x.PorcentajeDescuento).HasPrecision(5, 2);
        modelBuilder.Entity<Evento>().Property(x => x.PorcentajeDescuento).HasPrecision(5, 2);
        modelBuilder.Entity<PromocionPersonalizada>().Property(x => x.PorcentajeDescuento).HasPrecision(5, 2);

        // CANTIDADES E INVENTARIO: 18 dígitos, 3 decimales
        modelBuilder.Entity<DetalleOrdenCompra>().Property(x => x.Cantidad).HasPrecision(18, 3);
        modelBuilder.Entity<Insumo>().Property(x => x.StockActual).HasPrecision(18, 3);
        modelBuilder.Entity<Insumo>().Property(x => x.StockMinimo).HasPrecision(18, 3);
        modelBuilder.Entity<MovimientoInventario>().Property(x => x.Cantidad).HasPrecision(18, 3);
        modelBuilder.Entity<RecetaIngrediente>().Property(x => x.CantidadRequerida).HasPrecision(18, 3);

        // FACTOR DE PRECIO: 8 dígitos, 4 decimales
        modelBuilder.Entity<Tamano>().Property(x => x.FactorPrecio).HasPrecision(8, 4);

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
            new Rol { RolId = 2, Nombre = "Usuario", Descripcion = "Cliente de la pizzerÃ­a" }
        );
    }
}