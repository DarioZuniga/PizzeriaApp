using Microsoft.EntityFrameworkCore;
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

           
            //CONFIGURACIÓN DE AUTENTICACIÓN POR COOKIES (AGREGADO)
           
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "CookieAuth";
                options.DefaultSignInScheme = "CookieAuth";
                options.DefaultChallengeScheme = "CookieAuth";
            })
            .AddCookie("CookieAuth", options =>
            {
                options.Cookie.Name = "UserLoginCookie";
                options.LoginPath = "/Account/Login"; // Si intentan entrar al Home sin loguearse, los manda aquí
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<NetForumDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));

            // DP: Dependency Injection
            // SOLID: Dependency Inversion Principle
            //Su funcionalidad es registrar los repositorios 
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

           // 2. ORDEN DE LOS PERMISOS AGREGADO Y CORREGIDO
            app.UseAuthentication(); //  Primero el sistema pregunta: ¿Quién eres tú?
            app.UseAuthorization();  // Después el sistema revisa: ¿A qué tienes permiso?

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}