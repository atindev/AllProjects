using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Vocabularies;
using System.Linq;

namespace OdataLangTry
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);
            services.AddOData();

            //HttpContext httpContext = services.GetService<IHttpContextAccessor>().HttpContext;
            //string dbConn = httpContext.Request.Query["language"];
            //var httpRequest = httpContext.Request.Headers["AccountType"].ToString();
            //var httpRequestRoutes = httpContext.Request.RouteValues.Keys;
            //if (!string.IsNullOrEmpty(httpRequest) && httpRequest == "1")
            //{
            //    if (httpContext.Request.Method.Contains("GET"))
            //    {
            //        if (httpRequestRoutes.Contains("pharmaID"))
            //        {
            //            httpContext.Request.RouteValues["pharmaID"] = "33E29265-AE71-4445-85C1-5082153B2368";
            //        }
            //        if (httpRequestRoutes.Contains("clinicalTrialID"))
            //        {
            //            httpContext.Request.RouteValues["clinicalTrialID"] = "511F8128-E6FB-4AC0-8789-22DE6B72485C";
            //        }
            //
            //        dbConn = Configuration["connectionString:dbDemoDBConnection"];
            //    }
            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routeBuilder.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //    //endpoints.MapGet("/", async context =>
            //    //{
            //    //    await context.Response.WriteAsync("Hello World!");
            //    //});
            //});
        }

        private IEdmModel GetEdmModel()
        {
            EdmModel model = null;
            try
            {
                var odataBuilder = new ODataConventionModelBuilder() { Namespace = "Default", ContainerName = "West" };
                odataBuilder.EntitySet<Student>("Student");
                //EdmModel model = new EdmModel();
                model = (EdmModel)odataBuilder.GetEdmModel();

                EdmEntityType x = (EdmEntityType)model.FindType("Default.Student");
                string y = "Name";
                //(new Student()).GetType().FullName);//"OdataLangTry.Student");
                //model.SetAnnotationValue(x.FindProperty(y), x.Namespace, y, "a123");

                var term2 = new EdmTerm(x.Namespace, "Aggregation-Role", EdmCoreModel.Instance.GetString(true));
                var annotation3 = new EdmVocabularyAnnotation(x.FindProperty(y), term2, new EdmStringConstant("AHAH"));
                annotation3.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
                model.AddVocabularyAnnotation(annotation3);

                var term3 = new EdmTerm(x.Namespace, "Unit", EdmCoreModel.Instance.GetString(true));
                var annotation4 = new EdmVocabularyAnnotation(x.FindProperty(y), term3, new EdmStringConstant("KG"));
                annotation4.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
                model.AddVocabularyAnnotation(annotation4);
                var ys = model.VocabularyAnnotations.Where(xyd => xyd.Target == x.FindProperty(y) && xyd.Term.Name == "Label");

                model.SetDescriptionAnnotation(x.FindProperty(y), "ZCXC");
                model.SetLongDescriptionAnnotation(x.FindProperty(y), "A12345");

                //model.SetMimeType(x.FindProperty(y), "A123");
                //string xsd = model.GetLongDescriptionAnnotation(x.FindProperty(y));//, "A12345");

                //const string namespaceName = "http://my.org/schema";
                //var type = "My.Domain.Person";
                //const string localName = "Label";

                // this registers a "myns" namespace on the model
                //model.SetNamespaceAlias(x.Namespace, "MES");
                //model.SetNamespacePrefixMappings(new[] { new KeyValuePair<string, string>(x.Namespace, "MES") });

                // set a simple string as the value of the "MyCustomAttribute" annotation on the "RevisionDate" property
                //var stringType = EdmCoreModel.Instance.GetString(true);
                //var value = new EdmStringConstant(stringType, "SHOW");
                //model.SetAnnotationValue(x.FindProperty(y), "MES", localName, value);
                //model.SetAnnotationValue(((IEdmEntityType)model.FindType(type)).FindProperty("RevisionDate"), namespaceName, localName, value);
            }
            catch
            {
                /*EMPTY*/
            }
            return model;
        }
    }
}
