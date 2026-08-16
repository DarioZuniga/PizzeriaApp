using Microsoft.EntityFrameworkCore;
using NetForum.Business.Interfaces;
using NetForum.Business.Services;
using NetForum.Data;
using NetForum.Repository.Interfaces;
using NetForum.Repository.Repositories;

namespace NetForum.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CONFIGURACIÓN DE AUTENTICACIÓN POR COOKIES
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "CookieAuth";
                options.DefaultSignInScheme = "CookieAuth";
                options.DefaultChallengeScheme = "CookieAuth";
            })
            .AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "UserLoginCookie";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<NetForumDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // DP: Dependency Injection
            // SOLID: Dependency Inversion Principle - aquí se decide qué implementación concreta corresponde a
            // cada abstracción de la interfaz, y se inyecta en los controladores y servicios que la necesiten. 
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

            // DP: Service Layer Pattern + Dependency Injection
            builder.Services.AddScoped<IAuthService, AuthService>();

            // DP: Dependency Injection
            // SOLID: Dependency Inversion Principle - 
            // estos servicios se encargan de la lógica de negocio y dependen de los
            // repositorios para acceder a los datos
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>(); // Dependencias de Dashboard
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            builder.Services.AddScoped<ICategoriaService, CategoriaService>(); // Dependencias de Categoria
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();

            builder.Services.AddScoped<ILigaRepository, LigaRepository>(); // Dependencias de Liga
            builder.Services.AddScoped<ILigaService, LigaService>();

            builder.Services.AddScoped<IEquipoRepository, EquipoRepository>(); // Dependencias de Equipo
            builder.Services.AddScoped<IEquipoService, EquipoService>();

            builder.Services.AddScoped<IJugadorRepository, JugadorRepository>(); // Dependencias de Jugador
            builder.Services.AddScoped<IJugadorService, JugadorService>();

            builder.Services.AddScoped<ITemaRepository, TemaRepository>(); // Dependencias de Tema
            builder.Services.AddScoped<ITemaService, TemaService>();

            builder.Services.AddScoped<IRespuestaRepository, RespuestaRepository>();// Dependencias de Respuesta
            builder.Services.AddScoped<IRespuestaService, RespuestaService>();

            builder.Services.AddScoped<IEtiquetaRepository, EtiquetaRepository>(); // Dependencias de Etiqueta
            builder.Services.AddScoped<IEtiquetaService, EtiquetaService>();

            builder.Services.AddScoped<IReporteRepository, ReporteRepository>(); // Dependencias de Reporte
            builder.Services.AddScoped<IReporteService, ReporteService>();

            builder.Services.AddScoped<IReaccionRepository, ReaccionRepository>(); // Dependencias de Reaccion
            builder.Services.AddScoped<IReaccionService, ReaccionService>();

            builder.Services.AddScoped<IPerfilService, PerfilService>(); // Agregar el servicio de perfil

            builder.Services.AddScoped<ITemaEtiquetaRepository,TemaEtiquetaRepository>(); // Dependencias de TemaEtiqueta 
            builder.Services.AddScoped<ITemaEtiquetaService,TemaEtiquetaService>(); // Dependencias de TemaEtiqueta

            //--------------------------------------
            builder.Services.AddHttpClient<IFootballApiService, FootballApiService>(client =>
            {
                var baseUrl = builder.Configuration["FootballApi:BaseUrl"];
                var apiKey = builder.Configuration["FootballApi:ApiKey"];

                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    throw new InvalidOperationException("FootballApi:BaseUrl no está configurado.");
                }

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    throw new InvalidOperationException("FootballApi:ApiKey no está configurado.");
                }

                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("X-Auth-Token", apiKey);
            });
            //--------------------------------------

            var app = builder.Build();

            if (!app.Environment.IsDevelopment()) // Configuración de manejo de errores
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            // API de fútbol, aquí se configura el cliente HTTP para consumir la API externa de fútbol,
            // utilizando la URL base y la clave de API desde la configuración.

             builder.Services.AddHttpClient<IFootballApiService, FootballApiService>(
    client =>
    {
        var baseUrl =
            builder.Configuration["FootballApi:BaseUrl"];

        var apiKey =
            builder.Configuration["FootballApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "FootballApi:BaseUrl no está configurado.");
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "FootballApi:ApiKey no está configurado.");
        }

        client.BaseAddress =
            new Uri(baseUrl);

        client.DefaultRequestHeaders.Add(
            "X-Auth-Token",
            apiKey);
    });
        }

    }
}

/*Registro de un Typed Client (Cliente Tipado) mediante HttpClientFactory e Inyección de Dependencias en el archivo Program.cs.
¿Qué hace exactamento este código?

    Registra el servicio en el contenedor de dependencias (DI):
    Vincula la interfaz IFootballApiService con su implementación concreta FootballApiService, gestionando automáticamente el ciclo de vida del HttpClient.

    Lee la configuración (appsettings.json):
    Extrae la URL base (FootballApi:BaseUrl) y la clave de acceso (FootballApi:ApiKey) de tu archivo de configuración.

    Aplica validaciones de seguridad:
    Verifica si los valores existen. Si faltan la URL o la API Key, detiene la aplicación inmediatamente lanzando un InvalidOperationException para evitar fallos silenciosos 
en ejecución.

    Configura el cliente HTTP:

        BaseAddress: Establece la URL raíz para que todas las peticiones desde el servicio utilicen esa base
(ej. [https://api.football-data.org/v4/](https://api.football-data.org/v4/)).

   DefaultRequestHeaders: Agrega automáticamente el encabezado de autenticación X-Auth-Token
   con tu clave de API a todas las solicitudes salientes que realice este servicio.*/