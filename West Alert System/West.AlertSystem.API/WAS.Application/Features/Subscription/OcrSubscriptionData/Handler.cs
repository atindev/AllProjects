using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Settings;
using WAS.Application.Interface;

namespace WAS.Application.Features.Subscription.OcrSubscriptionData
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContextAdmin _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly AzureStorageSettings _azureStorageSettings;

        public Handler(
            IWasContextAdmin context,
            ILogger<Handler> logger,
            IMapper mapper,
            IOptions<AzureStorageSettings> azureStorageSettings
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _azureStorageSettings = azureStorageSettings.Value;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var Location = await _context.Locations
                                .FirstOrDefaultAsync(o => o.Name.Equals(request.Location), cancellationToken);
                var ocrSubscription = _mapper.Map<Domain.Entities.OcrSubscription>(request);
                ocrSubscription.FilePath = _azureStorageSettings.OcrSubscriptionBlobBaseUrl + request.FileName;
                ocrSubscription.LocationId = Location != null ? Location.Id : 0;
                await _context.OcrSubscriptions.AddAsync(ocrSubscription, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response{ Success = true};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
    }

}
