using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;

namespace WAS.API.Extensions
{
    /// <summary>
    /// Replace the version with API version
    /// </summary>
    public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        /// <summary>
        /// Replace the API version
        /// </summary>
        /// <param name="swaggerDoc">Swagger Open API Configuration</param>
        /// <param name="context">Document Filter Context</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var toReplaceWith = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
            {
                toReplaceWith.Add(key.Replace("v{version}", $"v{ swaggerDoc.Info.Version}", StringComparison.InvariantCulture), value);
            }

            swaggerDoc.Paths = toReplaceWith;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("v", "version"),
                    new UrlSegmentApiVersionReader());
            });

            return services;
        }

        public static IServiceCollection AddProblemDetailsHandlers(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Type = $"https://httpstatuses.com/{StatusCodes.Status422UnprocessableEntity}",
                        Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status422UnprocessableEntity),
                        Detail = "One or more validation errors occurred.",
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Instance = context.HttpContext.Request.Path
                    };

                    return new UnprocessableEntityObjectResult(problemDetails);
                };
            });

            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (context, exception) => false;
                options.Map<BadRequestException>(exception => new ProblemDetails
                {
                    Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status400BadRequest),
                    Detail = exception.Message,
                    Status = StatusCodes.Status400BadRequest
                });

                options.Map<NotFoundException>(exception => new ProblemDetails
                {
                    Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status404NotFound),
                    Detail = exception.Message,
                    Status = StatusCodes.Status404NotFound
                });

                options.Map<InternalServerErrorException>(exception => new ProblemDetails
                {
                    Title = ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                });

                options.OnBeforeWriteDetails = (context, problemDetails) =>
                {
                    problemDetails.Type = $"https://httpstatuses.com/{problemDetails.Status}";
                    problemDetails.Instance = context.Request.Path;
                };
            });

            return services;
        }
    }
}
