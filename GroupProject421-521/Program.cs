using GroupProject421_521.Data;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroupProject421_521
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var services = builder.Services;

            // Database: SQLite, using the connection string from configuration
            // (appsettings.json, user secrets or environment variables).
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            ConfigureAuthentication(services, configuration);

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddHttpClient();

            var app = builder.Build();

            // Create the database schema on first run.
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }

            // HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
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
            app.MapRazorPages();

            app.Run();
        }

        /// <summary>
        /// Sets up cookie authentication, plus Google, Facebook and Microsoft sign-in
        /// for any provider whose credentials are configured. Providers without
        /// credentials are skipped, so the app runs locally with no OAuth setup.
        /// </summary>
        private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var googleClientId = configuration["Google:ClientIdKey"];
            var googleSecret = configuration["Google:SecretKey"];

            if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleSecret))
            {
                services
                    .AddAuthentication(options =>
                    {
                        // Send sign-in challenges and permission checks to Google,
                        // and keep the signed-in session in a cookie.
                        options.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        options.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie()
                    .AddGoogleOpenIdConnect(options =>
                    {
                        options.ClientId = googleClientId;
                        options.ClientSecret = googleSecret;
                    });
            }
            else
            {
                services
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }

            var facebookAppId = configuration["Authentication:Facebook:AppId"];
            var facebookAppSecret = configuration["Authentication:Facebook:AppSecret"];
            if (!string.IsNullOrEmpty(facebookAppId) && !string.IsNullOrEmpty(facebookAppSecret))
            {
                services.AddAuthentication().AddFacebook(options =>
                {
                    options.AppId = facebookAppId;
                    options.AppSecret = facebookAppSecret;
                });
            }

            var microsoftClientId = configuration["Authentication:Microsoft:ClientId"];
            var microsoftClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            if (!string.IsNullOrEmpty(microsoftClientId) && !string.IsNullOrEmpty(microsoftClientSecret))
            {
                services.AddAuthentication().AddMicrosoftAccount(options =>
                {
                    options.ClientId = microsoftClientId;
                    options.ClientSecret = microsoftClientSecret;
                });
            }
        }
    }
}
