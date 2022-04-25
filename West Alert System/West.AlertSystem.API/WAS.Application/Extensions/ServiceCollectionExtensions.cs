using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Application.Common.Behaviours;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
            services.Configure<CosmosSettings>(configuration.GetSection("Cosmos"));
            services.Configure<TextAnalyticsSettings>(configuration.GetSection("TextAnalytics"));
            services.Configure<UserBlockedInterval>(options =>
            {
                options.UserBlockedTime = Convert.ToInt16(configuration["UserBlockedTime"]);
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
           
            return services;
        }
    }
}
