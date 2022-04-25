using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Common.Models;
using WAS.Application.Interface;

namespace WAS.Application.Features.Training.GetVideo
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;

        public Handler(
            ILogger<Handler> logger,
            IWasContext context,
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
                var Templates = await _context.VideoCategories
                                        .Include(i => i.TrainingVideos)
                                        .Include(i=>i.Language)
                                        .Where(category=>category.LanguageCode==request.LanguageCode)
                                        .IgnoreQueryFilters()                                        
                                        .ToListAsync(cancellationToken);

                var availableVideoLanguages = await _context.VideoCategories.Select(x => x.Language).Distinct().ToListAsync(cancellationToken);


                if (Templates == null)
                    return new Response();

                return new Response { VideoCategories = _mapper.Map<List<VideoCategory>>(Templates),AllLanguages=_mapper.Map<List<GlobalLanguage>>(availableVideoLanguages) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
