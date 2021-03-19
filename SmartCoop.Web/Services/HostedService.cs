﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCoop.Core.Coop;

namespace SmartCoop.Web.Services
{
    public class HostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public HostedService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var coop = scope.ServiceProvider.GetRequiredService<ICoop>();
            coop.Initialize();
            return Task.CompletedTask;
        }
    }
}