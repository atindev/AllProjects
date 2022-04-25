using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using WAS.Application.Extensions;
using WAS.API.Extensions;
using WAS.Infrastructure.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using System.Net.Http;
using Microsoft.AspNet.OData.Extensions;
using WAS.Infrastructure.OData;
using System.Linq;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Hellang.Middleware.ProblemDetails;

namespace WAS.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddVersioning();
            services.AddOData();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["AzADApp:TokenAuthority"];
                options.Audience = Configuration["AzADApp:Audience"];
            });

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
                options.OutputFormatters.RemoveType<TextOutputFormatter>();
            })
            .AddFluentValidation(option =>
                option.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.Formatting = Formatting.Indented;
            });

            services.AddProblemDetailsHandlers();

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddHttpClient();
            services.AddScoped(s => s.GetRequiredService<IHttpClientFactory>().CreateClient());

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(o => o.FullName);
                c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "West Alert System API",
                    Version = "1",
                    Description = "West Alert System API",
                });
            });
            services.AddApplicationInsightsTelemetry();

            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMvcCore(options =>
            {
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseProblemDetails();
            app.UseRouting();
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "West Alert System API V1");
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.Select().Filter().Expand().OrderBy().MaxTop(1000).Count();
                endpoints.MapODataRoute(
                              "odata",
                              "odata",
                               new WasEntityDataModel().GetEntityDataModel());
            });
        }
    }
}
