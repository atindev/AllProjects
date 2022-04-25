using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;

namespace WAS.Application.Features.Template.CreateCategory
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
                var newCategory = _mapper.Map<Domain.Entities.TemplateCategory>(request);

                if (request.Id == 0)
                {
                    newCategory.ModifiedDate = DateTime.UtcNow;
                    await _context.TemplateCategories.AddAsync(newCategory, cancellationToken);
                }
               
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Success = true, Id = newCategory.Id };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }

        }
    }
}
