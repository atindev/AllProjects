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

namespace WAS.Application.Features.Subscription.BlockUser
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IBlockedUserNotificationService _blockedUserNotificationService;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IBlockedUserNotificationService blockedUserNotificationService
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _blockedUserNotificationService = blockedUserNotificationService;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var blockedEntity = await _context.BlockedUsers
                     .FirstOrDefaultAsync(o => (o.OfficialEmail == request.OfficialEmail || o.EmployeeId==request.EmployeeId), cancellationToken);

                if (blockedEntity != null)
                {
                    blockedEntity.IsActive = false;
                }
                 
               var blockedUser = _mapper.Map<Domain.Entities.BlockedUser>(request);
               await _context.BlockedUsers.AddAsync(blockedUser, cancellationToken);
                
               await _context.SaveChangesAsync(cancellationToken);

                if (request.OfficialEmail != null && request.OfficialEmail != "")
                {
                    await _blockedUserNotificationService.SendBlockedUserNotification(request.OfficialEmail, request.Name, request.AttemptON);
                }

               return new Response { Success = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}

