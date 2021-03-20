using Microsoft.Extensions.DependencyInjection;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Services;
using SmartCoop.Infrastructure.Devices;
using SmartCoop.Infrastructure.Sensors;
using SmartCoop.Infrastructure.Services.MesageService;

namespace SmartCoop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Core.DependencyInjection).Assembly,  typeof(DependencyInjection).Assembly);

            services.AddSingleton<ICoop, Coop.Coop>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddTransient<IDevice, Door>();
            services.AddTransient<IDevice, Switch>();
            services.AddTransient<IDevice, Temperature>();
            services.AddTransient<IDevice, Level>();
            services.AddTransient<IDevice, PhotoCell>();

            return services;
        }
    }
}