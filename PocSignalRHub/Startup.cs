using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PocSignalRHub
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        //public static IConnectionManager ConnectionManager;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:5002");
            //}));
            //services.AddSignalR();
            services.AddSignalR().AddAzureSignalR("Endpoint=https://pccheck.service.signalr.net;AccessKey=BuCgnFblQ43gLYSESvDHrVxie792FZ78xCvq+vDLkPE=;Version=1.0;");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)//, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //ConnectionManager = serviceProvider.GetService<IConnectionManager>();
            //app.UseMvc();

            //app.UseRouting();
            //app.UseCors("CorsPolicy");

            //app.UseSignalR(routes => { routes.MapHub<OrderHub>("/orderHub"); });
            app.UseAzureSignalR(routes => { routes.MapHub<OrderHub>("/orderHub"); });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<OrderHub>("/orderHub");
            //    endpoints.MapControllers();
            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    await context.Response.WriteAsync("Hello World!");
            //    //});
            //});
        }
    }
}
