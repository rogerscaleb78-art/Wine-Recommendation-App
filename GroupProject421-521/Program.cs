using GroupProject421_521.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace GroupProject421_521
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            //if key-value pairs are nested in other objects (ex., Google), then you'll need a colon to separate them
            var googleClientId = builder.Configuration["Google:ClientIdKey"];
            var googleSecret = builder.Configuration["Google:SecretKey"];
            //var facebookClientId = builder.Configuration["Facebook:AppId"];
            //var facebookSecret = builder.Configuration["Facebook:AppSecret"];
            
            // Only configure Google OAuth if credentials are provided
            if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleSecret))
            {
                // This configures Google.Apis.Auth.AspNetCore3 for use in this app.
                builder.Services
                    .AddAuthentication(o =>
                    {
                        // This forces challenge results to be handled by Google OpenID Handler, so there's no
                        // need to add an AccountController that emits challenges for Login.
                        o.DefaultChallengeScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        // This forces forbid results to be handled by Google OpenID Handler, which checks if
                        // extra scopes are required and does automatic incremental auth.
                        o.DefaultForbidScheme = GoogleOpenIdConnectDefaults.AuthenticationScheme;
                        // Default scheme that will handle everything else.
                        // Once a user is authenticated, the OAuth2 token info is stored in cookies.
                        o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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
                // Configure basic authentication without external providers
                builder.Services
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();
            }

            
            var services = builder.Services;
            var configuration = builder.Configuration;

            // Only add Facebook authentication if credentials are provided
            var facebookAppId = configuration["Authentication:Facebook:AppId"];
            var facebookAppSecret = configuration["Authentication:Facebook:AppSecret"];
            if (!string.IsNullOrEmpty(facebookAppId) && !string.IsNullOrEmpty(facebookAppSecret))
            {
                services.AddAuthentication().AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = facebookAppId;
                    facebookOptions.AppSecret = facebookAppSecret;
                });
            }

            // Only add Microsoft authentication if credentials are provided
            var microsoftClientId = configuration["Authentication:Microsoft:ClientId"];
            var microsoftClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
            if (!string.IsNullOrEmpty(microsoftClientId) && !string.IsNullOrEmpty(microsoftClientSecret))
            {
                services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = microsoftClientId;
                    microsoftOptions.ClientSecret = microsoftClientSecret;
                });
            }

            // Use SQLite for local development to avoid Azure firewall issues
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=wineapp.db"));
            
            // Ensure database is created
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient(); //MAKE SURE TO ADD THIS FOR FUTURE WEBPAGES


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
    }
}
