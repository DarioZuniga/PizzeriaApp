using Microsoft.EntityFrameworkCore;
using PizzeriaApp.Business.Interfaces;
using PizzeriaApp.Business.Services;
using PizzeriaApp.Data;
using PizzeriaApp.Repository.Interfaces;
using PizzeriaApp.Repository.Repositories;

namespace PizzeriaApp.MVC
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

            builder.Services.AddDbContext<PizzeriaAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // Repositorios
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<ILigaRepository, LigaRepository>();
            builder.Services.AddScoped<IEquipoRepository, EquipoRepository>();
            builder.Services.AddScoped<IJugadorRepository, JugadorRepository>();
            builder.Services.AddScoped<ITemaRepository, TemaRepository>();
            builder.Services.AddScoped<IRespuestaRepository, RespuestaRepository>();
            builder.Services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();
            builder.Services.AddScoped<IReporteRepository, ReporteRepository>();
            builder.Services.AddScoped<IReaccionRepository, ReaccionRepository>();
            builder.Services.AddScoped<ITemaEtiquetaRepository, TemaEtiquetaRepository>();

            // Servicios
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<ILigaService, LigaService>();
            builder.Services.AddScoped<IEquipoService, EquipoService>();
            builder.Services.AddScoped<IJugadorService, JugadorService>();
            builder.Services.AddScoped<ITemaService, TemaService>();
            builder.Services.AddScoped<IRespuestaService, RespuestaService>();
            builder.Services.AddScoped<IEtiquetaService, EtiquetaService>();
            builder.Services.AddScoped<IReporteService, ReporteService>();
            builder.Services.AddScoped<IReaccionService, ReaccionService>();
            builder.Services.AddScoped<IPerfilService, PerfilService>();
            builder.Services.AddScoped<ITemaEtiquetaService, TemaEtiquetaService>();

            // ✅ NO hay bloque de FootballApi aquí

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
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
        }
    }
}