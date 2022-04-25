using Microsoft.AspNet.OData.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Infrastructure.Persistence;
using WAS.Infrastructure.Services;

namespace WAS.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WasContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"]); 
            });

            services.AddDbContext<WasContextAdmin>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStringAdmin"]);
            });

            services.Configure<AzureStorageSettings>(configuration.GetSection("AzStorage"));
            services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));
            services.Configure<GroupRestorationCount>(options =>
            {
                options.DeletedGroupRententionDays =  Convert.ToInt16(configuration["DeletedGroupRententionDays"]);
            });
            services.Configure<GraphSettings>(configuration.GetSection("Graph"));

            services.AddScoped<IWasContext, WasContext>();
            services.AddScoped<IWasContextAdmin, WasContextAdmin>();
            services.AddScoped<IAzureStorageService, AzureStorageService>();
            services.AddScoped<ITimeParser, TimeParser>();
            services.AddScoped<IGenerateExpression, GenerateExpression>();
            services.AddScoped<ITwilioService, TwilioService>();
            services.AddScoped<ISubscriptionConfirmationService, SubscriptionConfirmationService>();
            services.AddScoped<IAlertAdminService, AlertAdminService>();
            services.AddScoped<IShareSurveyService, ShareSurveyService>();
            services.AddScoped<IBlockedUserNotificationService, BlockedUserNotificationService>();
            services.AddTransient<IGroupChangeNotificationService, GroupChangeNotificationService>();
            services.AddTransient<INotificationApprovalAlert, NotificationApprovalAlert>();
            services.AddTransient<IGraphService, GraphService>();
            services.AddTransient<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<ICosmosProvider, CosmosProvider>();
            services.AddTransient<ITextAnalyticsProvider, TextAnalyticsProvider>();


            services.AddScoped<IDevopsService, DevopsService>();
            services.AddScoped<IBlobTransactionService, BlobTransactionService>();
            services.AddScoped<IEmailFormatService, EmailFormatService>();

            return services;
        }
    }
}
