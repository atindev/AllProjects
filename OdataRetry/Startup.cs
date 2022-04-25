using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.Edm;
using OdataRetry.Common;
using OdataRetry.Middleware.Routing;
using OdataRetry.Model;
using System;
using West.Manufacturing.Repository.Implementations;
using West.Manufacturing.Repository.Interfaces;

namespace OdataRetry
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<DbContext, MfgSystemsDbContext>();
            services.AddScoped(typeof(IEnterpriseRepository<>), typeof(EnterpriseSQLServerRepository<>));
            services.AddDbContext<MfgSystemsDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("MFGSYSDB"), opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds))
                );

            services.AddOData();
            services.AddMvc().ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()));
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(builder =>
            {
                builder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                builder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        private IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "WebAPI";
            builder.ContainerName = "DefaultContainer";
            //builder.EnableLowerCamelCase();

            foreach (Type item in MfgCommon.GetTypesInNamespace())
            {
                EntityTypeConfiguration entityType = builder.AddEntityType(item);
                builder.AddEntitySet(item.Name, entityType);
            }
            //builder.EntitySet<DTOOperationProcess>("DTOOperationProcess");
            //builder.EntitySet<DTOProcessInstructions>("DTOProcessInstructions");
            //builder.EntitySet<DTOEquipementOperation>("DTOEquipementOperation");
            //builder.EntitySet<DTOEquipmentCategory>("DTOEquipmentCategory");
            //builder.EntitySet<DTOEquipmentInstructions>("DTOEquipmentInstructions");

            //builder.Function("GetEquipmentCategories")
            //    .Returns<Task<List<DTOEquipmentCategory>>>()
            //    .Parameter<string>("PlantID");

            //builder.Function("GetEquipmentClassifications")
            //    .Returns<Task<List<DTOEquipmentCategory>>>()
            //    .Parameter<string>("PlantID");

            //builder.Function("GetPlantIds")
            //    .Returns<List<string>>();

            return builder.GetEdmModel();
        }
    }
}
