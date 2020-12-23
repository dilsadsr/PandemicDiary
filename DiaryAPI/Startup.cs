using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiaryAPI.Data;
using DiaryAPI.Extentions;
using DiaryAPI.RabbitMQ;
using DiaryAPI.Repositories;
using DiaryAPI.Repositories.Interfaces;
using DiaryAPI.Settings;
using EventBusRabbitMq.Producer;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DiaryAPI
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

            // settings
            services.Configure<DiaryDatabaseSettings>(Configuration.GetSection(nameof(DiaryDatabaseSettings)));
            services.AddSingleton<IDiaryDatabaseSettings>(d => d.GetRequiredService<IOptions<DiaryDatabaseSettings>>().Value);

            // dependincies
            services.AddTransient<IDiaryContext, DiaryContext>();
            services.AddTransient<IDiaryNoteRepository, DiaryNoteRepository>();
            services.AddAutoMapper(typeof(Startup));

            // Swagger
            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Diary API", Version = "v1" });
            });

            // rabbitmq
            services.AddSingleton<IRabbitMqConnection>(s =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBus:HostName"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                {
                    factory.UserName = Configuration["EventBus:UserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                {
                    factory.Password = Configuration["EventBus:Password"];
                }

                return new RabbitMqConnection(factory);
            });

            services.AddSingleton<EventBusRabbitMqProducer>();

            services.AddSingleton<EventBusRabbitMqConsumer>();

            services.AddCors(c => { c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRabbitListener();

            app.UseSwagger();
            app.UseSwaggerUI(u => u.SwaggerEndpoint("/swagger/v1/swagger.json", "Diary API V1"));
        }
    }
}
