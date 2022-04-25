using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WAS.Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _stopWatch;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(ILogger<TRequest> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _stopWatch.Start();

            var response = await next();

            _stopWatch.Stop();

            if(_stopWatch.ElapsedMilliseconds > 1000)
                _logger.LogWarning("Long running request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}",
                    typeof(TRequest).FullName, _stopWatch.ElapsedMilliseconds, request);

            return response;
        }
    }
}
