using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OpenApExperiments.Data;
using OpenApExperiments.Infrastructure;

namespace Company.WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();

            // Add ant Design
            builder.Services.AddAntDesign();

            // Access user secrets
            var apiToken = builder.Configuration["apiToken"];
            var settings = new AppSettings()
            {
                ApiToken = apiToken,
            };
            builder.Services.AddSingleton<IAppSettings>(settings);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}