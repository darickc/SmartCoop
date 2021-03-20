using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCoop.Core.Coop;
using SmartCoop.Core.Services;

namespace SmartCoop.Web.Services
{
    public class HostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
            await messageService.Connect();
            var coop = scope.ServiceProvider.GetRequiredService<ICoop>();
            coop.Load();
            coop.Initialize();
        }
    }
}