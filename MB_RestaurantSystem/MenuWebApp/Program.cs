using MenuWebApp.Services;
using Microsoft.AspNetCore.Builder.Extensions;
using Shared.Options;

namespace MenuWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<GcpOptions>(
                builder.Configuration.GetSection("Gcp"));

            builder.Services.Configure<GoogleAuthOptions>(
                builder.Configuration.GetSection("GoogleAuth"));

            builder.Services.AddScoped<IStorageService, StorageService>();
            builder.Services.AddScoped<IFirestoreService, FirestoreService>();
            builder.Services.AddScoped<IPubSubService, PubSubService>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromHours(2);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
