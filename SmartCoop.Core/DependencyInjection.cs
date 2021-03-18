using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace SmartCoop.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationCore(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(assembly);
            return services;
        }
    }
}