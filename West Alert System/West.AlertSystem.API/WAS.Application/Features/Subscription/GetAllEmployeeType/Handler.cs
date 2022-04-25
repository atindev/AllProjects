using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Subscription.GetAllEmployeeType
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var EmployeeTypes = await _context.Subscriptions
                    .Select(x=>x.EmployeeType).Distinct()
                    .ToListAsync(cancellationToken);
                EmployeeTypes.RemoveAll(i => i == null);
                if (EmployeeTypes == null)
                    return new Response();

                var responseEmployeeTypes = _mapper.Map<List<string>>(EmployeeTypes);

                return new Response { EmployeeTypes = responseEmployeeTypes };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }

        }
    }
}
