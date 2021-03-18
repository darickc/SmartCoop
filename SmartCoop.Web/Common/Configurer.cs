using MatBlazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCoop.Core;
using SmartCoop.Core.Coop;
using SmartCoop.Infrastructure;
using SmartCoop.Infrastructure.Coop;
using SmartCoop.Infrastructure.Devices;
using SmartCoop.Infrastructure.Sensors;
using SmartCoop.Web.Services;

namespace SmartCoop.Web.Common
{
    public static class Configurer
    {
        public static void ConfigureWeb(this IServiceCollection services, IConfiguration config)
        {
            ICoop coop = new Coop();
            config.GetSection("Coop").Bind(coop);
            coop.Devices.Add(new Door{Name = "Door"});
            coop.Devices.Add(new Temperature{Name = "Water Temp"});
            coop.Devices.Add(new Level{Name = "Water Level"});
            coop.Devices.Add(new Level { Name = "Food Level" });
            coop.Devices.Add(new Switch { Name = "Light" });
            coop.Devices.Add(new Switch { Name = "Water Heater" });

            services.AddSingleton(coop);
            // get all interfaces settings implements and put into DI
            // typeof(Coop).GetInterfaces().ToList().ForEach(t => services.AddSingleton(t, settings));

            services.AddMatBlazor();
            services.AddApplicationCore();
            services.AddInfrastructure();

            services.AddHostedService<HostedService>();
        }
    }
}