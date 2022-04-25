using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Security.Claims;
using Vereyon.Web;
using WAS.Application.Common.Settings;
using WAS.Application.Extensions;
using WAS.Infrastructure.Extensions;
using WAS.Web.Extension;
using WAS.Web.Models;

namespace WAS.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddAzureAd(options => Configuration.Bind("AzureAd", options))
           .AddCookie();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("WASAdmin", policy => policy.RequireRole("WASAdmin", "GlobalAdministrator"));
                options.AddPolicy("SurveyAdmin", policy => policy.RequireRole("WASAdmin", "GlobalAdministrator", "SurveyAdmin"));
                options.AddPolicy("Approver", policy => policy.RequireClaim(ClaimTypes.Role, "Approver"));
                options.AddPolicy("CommunicationTeam", policy => policy.RequireRole("CommunicationTeam", "Approver", "WASAdmin", "GlobalAdministrator"));
            });

            IMvcBuilder builder = services.AddRazorPages();
            builder.AddRazorRuntimeCompilation();

            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);

            services.AddControllersWithViews();
            services.AddProblemDetailsHandlers();
            services.AddFlashMessage();
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddApplicationInsightsTelemetry();
            builder.Services.Configure<AzureStorageSettings>(Configuration.GetSection("AzStorage"));
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(60);  
            });
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/WAS/Forbidden/Error", "?errorStatusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseNotyf();

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Configuration["SyncfusionLicence:Key"]);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
