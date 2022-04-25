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

namespace WAS.Application.Features.Group.CreateUpdate
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IOptions<GroupRestorationCount> _groupOptions;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IOptions<GroupRestorationCount> options
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _groupOptions = options;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            try
            {
                var existingGroup = await _context.Groups
                                      .IgnoreQueryFilters()
                           .FirstOrDefaultAsync(b => (
                           (b.IsActive || (b.DeletedDate != null && b.DeletedDate.Value.AddDays(_groupOptions.Value.DeletedGroupRententionDays) > DateTime.UtcNow)) &&
                           (b.Name.Trim().ToUpper() == request.Name.Trim().ToUpper())
                           ), cancellationToken);
                if (request.Id[0] != 0 && existingGroup.Id == request.Id[0] && existingGroup.Name == request.Name)
                {
                        existingGroup = null;
                }

                if (existingGroup != null)
                    return new Response { Success = true, Id = 0, IsNameExist = true };

                var newGroup = _mapper.Map<Domain.Entities.Group>(request);
                if (request.Id[0] == 0)
                {
                     newGroup.ModifiedDate = DateTime.UtcNow;
                     await _context.Groups.AddAsync(newGroup, cancellationToken);
                }
                else
                {
                    var result = await _context.Groups
                   .SingleOrDefaultAsync(b => b.Id == request.Id[0], cancellationToken);

                    if (result == null)
                        throw new BadRequestException("Group not found");

                    var groupEntity = _mapper.Map(request, result);
                    _context.Groups.Attach(groupEntity).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync(cancellationToken);
                return new Response { Success = true, Id = newGroup.Id };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message);
            }
        }
    }
}
