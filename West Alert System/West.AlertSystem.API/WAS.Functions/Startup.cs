using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Functions;
using WAS.Infrastructure.Persistence;
using WAS.Infrastructure.Services;

[assembly: FunctionsStartup(typeof(Startup))]

namespace WAS.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<WasContextAdmin>(options =>
            {
                options.UseSqlServer(
                    Environment.GetEnvironmentVariable("ConnectionStringAdmin"));
            });

            var config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                           .AddEnvironmentVariables()
                           .Build();
            builder.Services.Configure<TwilioSettings>(config.GetSection("Twilio"));
            builder.Services.Configure<AzureStorageSettings>(config.GetSection("AzStorage"));
            builder.Services.Configure<GraphSettings>(config.GetSection("Graph"));
            builder.Services.Configure<CosmosSettings>(config.GetSection("Cosmos"));


            builder.Services.AddOptions();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<IWasContextAdmin, WasContextAdmin>();
            builder.Services.AddScoped<ITwilioService, TwilioService>();
            builder.Services.AddScoped<IAzureStorageService, AzureStorageService>();

            builder.Services.AddScoped<IDevopsService, DevopsService>();
            builder.Services.AddScoped<IBlobTransactionService, BlobTransactionService>();

            builder.Services.AddTransient<IGraphService, GraphService>();
            builder.Services.AddTransient<IGraphAuthProvider, GraphAuthProvider>();
            builder.Services.AddTransient<IEmailFormatService, EmailFormatService>();
            builder.Services.AddTransient<ICosmosProvider, CosmosProvider>();

        }
    }
}
