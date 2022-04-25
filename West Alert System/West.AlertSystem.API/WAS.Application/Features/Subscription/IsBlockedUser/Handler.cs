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
using WAS.Application.Interface.Services;
using ObjectsComparer;
using System.Linq;

namespace WAS.Application.Features.Subscription.IsBlockedUser
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IOptions<UserBlockedInterval> _IntervalOptions;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IOptions<UserBlockedInterval> options
            )
        {
            _context = context;
            _logger = logger;
            _IntervalOptions = options;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var blockedUserEntity = await _context.BlockedUsers
                     .FirstOrDefaultAsync(o => ((o.OfficialEmail == request.EmailorEmployeeId || o.EmployeeId==request.EmailorEmployeeId) && 
                     (o.CreatedDate.AddMinutes(_IntervalOptions.Value.UserBlockedTime) >DateTime.UtcNow)), cancellationToken);
                 
                if (blockedUserEntity != null)
                     return new Response { IsBlocked = true };
                else
                    return new Response { IsBlocked = false };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}

