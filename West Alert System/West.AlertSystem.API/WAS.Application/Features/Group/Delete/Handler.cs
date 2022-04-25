using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Domain.Entities;

namespace WAS.Application.Features.Group.Delete
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
                var result = await _context.Groups
                    .SingleOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                if (result == null)
                    throw new BadRequestException("Group not found");

                
                result.ModifiedBy = request.ModifiedBy;
                _context.Groups.Remove(result);
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
