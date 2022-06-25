using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Factory;
using RabbitMQ.Common.Messaging.Settings;
using RabbitMQ.Consumer.BackgroundServices;
using RabbitMQ.Service.Implements;
using RabbitMQ.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer
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
            // RabbitMQSetting
            var rabbitMQSetting = new RabbitMQSettings();
            Configuration.GetSection("RabbitMQSettings").Bind(rabbitMQSetting);
            services.AddSingleton(rabbitMQSetting);

            services.AddControllersWithViews();

            services.AddTransient<IRabbitMQHelper, RabbitMQHelper>();

            services.AddHostedService<MessageBackgroundService>();

            services.AddTransient<IBusFactory, BusFactory>();

            services.AddTransient<IHouseService, HouseService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}