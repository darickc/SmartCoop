using System.Linq;
using MatBlazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCoop.Core;
using SmartCoop.Infrastructure;
using SmartCoop.Web.Services;

namespace SmartCoop.Web.Common
{
    public static class Configurer
    {
        public static void ConfigureWeb(this IServiceCollection services, IConfiguration config)
        {
            Settings settings = new Settings();
            config.GetSection("settings").Bind(settings);
            // get all interfaces settings implements and put into DI
            typeof(Settings).GetInterfaces().ToList().ForEach(t => services.AddSingleton(t, settings));

            services.AddMatBlazor();
            services.AddApplicationCore();
            services.AddInfrastructure();

            services.AddHostedService<HostedService>();
        }
    }
}