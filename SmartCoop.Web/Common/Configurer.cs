using MatBlazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartCoop.Core;
using SmartCoop.Core.Coop;
using SmartCoop.Infrastructure;
using SmartCoop.Infrastructure.Coop;
using SmartCoop.Web.Services;

namespace SmartCoop.Web.Common
{
    public static class Configurer
    {
        public static void ConfigureWeb(this IServiceCollection services, IConfiguration config)
        {
            
            ICoop coop = new Coop();
            coop.Load();
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