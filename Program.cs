using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuokkaServiceRegistry.Authorization;
using QuokkaServiceRegistry.Data;
using QuokkaServiceRegistry.Services;

namespace QuokkaServiceRegistry;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = BuildConnectionString(builder.Configuration);
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        // Configure Authentication
        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.LogoutPath = "/Account/Logout";
            options.AccessDeniedPath = "/Account/AccessDenied";
            options.ExpireTimeSpan = TimeSpan.FromHours(24);
            options.SlidingExpiration = true;
        });

        // Add Google authentication only if credentials are configured
        var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
        var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        
        if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
        {
            // Update default challenge scheme to Google when available
            builder.Services.Configure<AuthenticationOptions>(options =>
            {
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            });

            authBuilder.AddGoogle(options =>
            {
                options.ClientId = googleClientId;
                options.ClientSecret = googleClientSecret;
                options.SaveTokens = true;
                
                // Ensure we get the email claim
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Email, "email");
                options.ClaimActions.MapJsonKey(System.Security.Claims.ClaimTypes.Name, "name");
                
                // Force account selection prompt for multiple Google accounts
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
                    return Task.CompletedTask;
                };
            });
        }

        // Register services
        builder.Services.AddScoped<IAuthorizedUsersService, AuthorizedUsersService>();
        
        // Configure Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new EmailAuthorizationRequirement())
                .Build();
        });
        
        builder.Services.AddScoped<IAuthorizationHandler, EmailAuthorizationHandler>();
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

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
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            // Ensure database is created and migrations are applied
            context.Database.Migrate();
            
            SeedData.SeedLicenses(context);
            SeedData.SeedCostCenters(context);
        }

        app.Run();
    }

    private static string BuildConnectionString(IConfiguration configuration)
    {
        // Try environment variables first (for Kubernetes deployment)
        var host = configuration["Database:Host"];
        var database = configuration["Database:Name"];
        var username = configuration["Database:Username"];
        var password = configuration["Database:Password"];

        if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(database) && 
            !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            return $"Host={host};Database={database};Username={username};Password={password}";
        }

        // Fallback to traditional connection string from appsettings.json
        var fallbackConnectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(fallbackConnectionString))
        {
            return fallbackConnectionString;
        }

        throw new InvalidOperationException(
            "Database connection configuration not found. " +
            "Please configure either Database:Host, Database:Name, Database:Username, Database:Password " +
            "environment variables or ConnectionStrings:DefaultConnection in appsettings.json");
    }
}