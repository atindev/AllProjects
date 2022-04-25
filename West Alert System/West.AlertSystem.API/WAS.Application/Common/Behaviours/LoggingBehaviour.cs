using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WAS.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        /// <summary>
        /// Inititalize a new Mediatr logging behaviour
        /// </summary>
        /// <param name="logger"></param>
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Request: {Name} {@Request}",
                typeof(TRequest).FullName, request);

            var response = await next();

            _logger.LogInformation("Response: {Name} {@Request}", 
                typeof(TResponse).FullName, response);

            return response;
        }
    }
}
