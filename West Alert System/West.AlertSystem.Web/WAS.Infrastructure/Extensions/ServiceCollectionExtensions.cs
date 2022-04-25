using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WAS.Application.Interface.Services;
using WAS.Infrastructure.Helpers;
using WAS.Infrastructure.Services;
using WAS.Infrastructure.Settings;

namespace WAS.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureAdSettings>(configuration.GetSection("AzureAd"));

            services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();

            services.AddScoped<IServiceBase, ServiceBase>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IGraphService, GraphService>();
            services.AddScoped<ITriangularValidationService, TriangularValidationService>();

            return services;
        }
    }
}
