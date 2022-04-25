using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WAS.Application.Common.Behaviours;
using WAS.Application.Common.Settings;

namespace WAS.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WasApiSettings>(configuration.GetSection("WASApiSettings"));
            services.Configure<FileProperties>(configuration.GetSection("FileProperties"));
            services.Configure<Recaptcha>(configuration.GetSection("Recaptcha"));
            services.Configure<GraphSettings>(configuration.GetSection("Graph"));
            services.Configure<AzureConfig>(configuration.GetSection("AzureConfig"));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.Configure<UserBlockedInterval>(options =>
            {
                options.UserBlockedTime = Convert.ToInt16(configuration["UserBlockedTime"]);
            });

            return services;
        }
    }
}
