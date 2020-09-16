using ChatAPI.Data.Models;
using ChatAPI.Middleware.Extension;
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
            services.AddDbContext<ChatDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("ChatDb"));
            });
            services.AddScoped<ChatDbContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, ChatDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
            context.Users.Add(new Data.Models.Entities.User { IsActive = true, CreatedAt = DateTime.Now, RoomId = 2, Username = "test1" });
            context.SaveChanges();

        }
        
    }
}
