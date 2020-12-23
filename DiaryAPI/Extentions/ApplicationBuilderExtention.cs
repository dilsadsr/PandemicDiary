using System;
using DiaryAPI.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DiaryAPI.Extentions
{
    public static class ApplicationBuilderExtention
    {
        public static EventBusRabbitMqConsumer Listener { get; set; }

        // custome middleware
        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusRabbitMqConsumer>();

            var life =app.ApplicationServices.GetService<IHostApplicationLifetime>();

            life.ApplicationStarted.Register(OnStart);
            // life.ApplicationStarted.Register(OnStop);
            return app;
        }

        //private static void OnStop()
        //{
        //    Listener.Disconnect();
        //}

        private static void OnStart()
        {
            Listener.Consume();
        }
    }
}
