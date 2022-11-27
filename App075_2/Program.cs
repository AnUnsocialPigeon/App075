using App075_2.Data;
using App075_2.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using static App075_2.Interfaces.CookieClass;


namespace App075_2 {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor()
                .AddHubOptions(options => {
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(60);
                    options.HandshakeTimeout = TimeSpan.FromSeconds(30);
                }); 
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddResponseCompression(opts => {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
            builder.Services.AddScoped<ICookie, Cookie>();

            var app = builder.Build();
            app.UseResponseCompression();

            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapHub<ChatHub>("/chathub");
            app.MapHub<PsychHub>("/psych");
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}