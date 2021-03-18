using Microsoft.Extensions.DependencyInjection;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Devices;
using SmartCoop.Core.Sensors;
using SmartCoop.Core.Sensors.Temperature;
using SmartCoop.Infrastructure.Devices;
using SmartCoop.Infrastructure.Sensors;

namespace SmartCoop.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Core.DependencyInjection).Assembly,  typeof(DependencyInjection).Assembly);

            // services.AddSingleton<ICoop, Coop.Coop>();
            services.AddTransient<IDoor, Door>();
            services.AddTransient<ISwitch, Switch>();
            services.AddTransient<ITemperature, Temperature>();
            services.AddTransient<ILevel, Level>();
            services.AddTransient<IPhotoCell, PhotoCell>();

            return services;
        }
    }
}