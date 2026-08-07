using Microsoft.EntityFrameworkCore;
using NetForum.Data.Entities;

namespace NetForum.Data;

public class NetForumDbContext : DbContext
{
    public NetForumDbContext(DbContextOptions<NetForumDbContext> options)
        : base(options)
    {
    }

    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Liga> Ligas => Set<Liga>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Tema> Temas => Set<Tema>();
    public DbSet<Respuesta> Respuestas => Set<Respuesta>();
    public DbSet<TipoReaccion> TiposReaccion => Set<TipoReaccion>();
    public DbSet<Reaccion> Reacciones => Set<Reaccion>();
    public DbSet<Reporte> Reportes => Set<Reporte>();
    public DbSet<Notificacion> Notificaciones => Set<Notificacion>();
    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();
    public DbSet<TemaEtiqueta> TemaEtiquetas => Set<TemaEtiqueta>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // DP: Fluent API - centraliza el mapeo de entidades en un solo lugar en vez
        // de dispersarlo con Data Annotations en cada clase de entidad.
        // SOLID: Single Responsibility Principle - el DbContext solo configura
        // persistencia que no contiene lógica de negocio.

        modelBuilder.Entity<Estado>().ToTable("Estados");
        modelBuilder.Entity<Rol>().ToTable("Roles");
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<Categoria>().ToTable("Categorias");
        modelBuilder.Entity<Liga>().ToTable("Ligas");
        modelBuilder.Entity<Equipo>().ToTable("Equipos");
        modelBuilder.Entity<Jugador>().ToTable("Jugadores");
        modelBuilder.Entity<Tema>().ToTable("Temas");
        modelBuilder.Entity<Respuesta>().ToTable("Respuestas");
        modelBuilder.Entity<TipoReaccion>().ToTable("TiposReaccion");
        modelBuilder.Entity<Reaccion>().ToTable("Reacciones");
        modelBuilder.Entity<Reporte>().ToTable("Reportes");
        modelBuilder.Entity<Notificacion>().ToTable("Notificaciones");
        modelBuilder.Entity<Etiqueta>().ToTable("Etiquetas");
        modelBuilder.Entity<TemaEtiqueta>().ToTable("TemaEtiquetas");

        modelBuilder.Entity<TemaEtiqueta>()
            .HasIndex(te => new { te.TemaId, te.EtiquetaId })
            .IsUnique();

        modelBuilder.Entity<Reaccion>()
            .HasIndex(r => new { r.UsuarioId, r.TemaId, r.TipoReaccionId })
            .IsUnique();

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
            new Rol { RolId = 2, Nombre = "Moderador", Descripcion = "Gestiona contenido del foro" },
            new Rol { RolId = 3, Nombre = "Usuario", Descripcion = "Usuario general del foro" }
        );

        modelBuilder.Entity<TipoReaccion>().HasData(
            new TipoReaccion { TipoReaccionId = 1, Nombre = "Me gusta" },
            new TipoReaccion { TipoReaccionId = 2, Nombre = "Golazo" },
            new TipoReaccion { TipoReaccionId = 3, Nombre = "Polémico" }
        );

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { CategoriaId = 1, EstadoId = 1, Nombre = "Champions League", Descripcion = "Debates sobre la Champions League" },
            new Categoria { CategoriaId = 2, EstadoId = 1, Nombre = "Premier League", Descripcion = "Foro sobre fútbol inglés" },
            new Categoria { CategoriaId = 3, EstadoId = 1, Nombre = "Liga Promerica", Descripcion = "Foro sobre fútbol de Costa Rica" }
        );

        modelBuilder.Entity<Liga>().HasData(
            new Liga { LigaId = 1, Nombre = "La Liga", Pais = "España" },
            new Liga { LigaId = 2, Nombre = "Premier League", Pais = "Inglaterra" },
            new Liga { LigaId = 3, Nombre = "Liga Promerica", Pais = "Costa Rica" }
        );

        modelBuilder.Entity<Etiqueta>().HasData(
            new Etiqueta { EtiquetaId = 1, Nombre = "Fichajes" },
            new Etiqueta { EtiquetaId = 2, Nombre = "VAR" },
            new Etiqueta { EtiquetaId = 3, Nombre = "Clásico" }
        );
    }
}