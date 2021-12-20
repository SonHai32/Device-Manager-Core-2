using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceManagerCore.Hubs;
using DeviceManagerCore.Respository;

namespace DeviceManagerCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeviceManagerCore", Version = "v1" });
            });
            services.AddSignalR();
            services.AddTransient<IDeviceRespository, DeviceRespository>();
            services.AddCors(options =>

            {
                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.
                    AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();

                });

            });
            services.AddCors(options =>
           {
               options.AddPolicy("HubPolicy", policy =>
               {
                   policy.
                   WithOrigins("http://192.168.127.102:3000", "http://localhost:3000", "*")
            .AllowAnyHeader()
            .WithMethods("GET", "POST")
            .AllowCredentials();

               });

           });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DeviceManagerCore v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("ClientPermission");

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DeviceHub>("/deviceHub").RequireCors("HubPolicy");
            });
        }
    }
}
