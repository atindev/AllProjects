using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Application.Interface.Services;
using WAS.Application.Common.Models;
using WAS.Application.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace WAS.Application.Features.Notification.ApproveReject
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger
            )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _context.Notifications
                    .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);

                switch (request.ApprovalLevel)
                {
                    case Common.Enum.ApprovalLevel.First:
                        notification.ApprovedBy = request.ApprovedBy;
                        notification.ApprovedDate = DateTime.UtcNow;
                        notification.Status = request.Status;
                        notification.Comment = request.Comment;
                        notification.ApprovedTimeZone = request.ApprovedTimeZone;
                        break;
                    case Common.Enum.ApprovalLevel.Second:
                        notification.FinalApprovalBy = request.ApprovedBy;
                        notification.FinalApprovalDate = DateTime.UtcNow;
                        notification.Status = request.Status;
                        notification.Comment = request.Comment;
                        notification.FinalApprovalTimeZone = request.FinalApprovalTimeZone;
                        break;
                }

                await _context.SaveChangesAsync(cancellationToken);

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
