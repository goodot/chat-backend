using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Middleware.Extension;
using ChatAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ChatAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddWebSocketConnectionManager();
            var server = "localhost";
            var port = "1433";
            var user = "SA";
            var password = "Pa55w0rd2019";
            var database = "ChatDb";

            var connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}";
            services.AddDbContext<ChatDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });
            services.AddScoped<ChatDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.MigrateDatabase();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseCors();
            app.UseWebSockets();
            app.UseWebSocketServer();

        }
        
    }
}
