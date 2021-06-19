using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.BackgroundServices;
using RabbitMQ.Common.Messaging;
using RabbitMQ.Common.Messaging.Factory;
using RabbitMQ.Common.Messaging.Settings;
using RabbitMQ.Service;
using RabbitMQ.Service.Implements;
using RabbitMQ.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQ
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

            services.AddTransient<IRabbitMQHelper, RabbitMQHelper>()
                    .Decorate<IRabbitMQHelper, AdvancedRabbitMQHelper>();

            services.AddHostedService<MessageBackgroundService>();

            services.AddTransient<IBusFactory, BusFactory>();

            services.AddTransient<IHouseService, HouseService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RabbitMQ", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RabbitMQ v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}