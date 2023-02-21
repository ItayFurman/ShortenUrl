using Microsoft.EntityFrameworkCore;
using ProjectUrlShort.Data;
using System;

namespace ProjectUrlShort
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<UrlDBcontext>(x =>
            x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
            );


			var connectionString = builder.Configuration.GetConnectionString("SqlServer")
			  ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

			builder.Services.ConfigureIdentity(connectionString);
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //using (var scope = app.Services.CreateScope())
            //{
            //    using var db = scope.ServiceProvider.GetRequiredService<UrlDBcontext>();
            //    db!.Database.Migrate();    
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }
    }
}