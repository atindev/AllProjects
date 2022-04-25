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

namespace WAS.Application.Features.Training.GetVideoById
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
                Response response = new Response();

                var trainingVideoEntity = await _context.TrainingVideos
                                        .Include(i => i.VideoCategory)
                                            .ThenInclude(i => i.TrainingVideos)
                                        .IgnoreQueryFilters()
                                        .FirstOrDefaultAsync(v => v.Id == request.VideoId, cancellationToken);

                if (trainingVideoEntity == null)
                    return new Response();

                response.TrainingVideo = _mapper.Map<TrainingVideo>(trainingVideoEntity);
                var videos = trainingVideoEntity.VideoCategory.TrainingVideos.Where(i => i.Id != request.VideoId).ToList();
                response.Videos = _mapper.Map<List<TrainingVideo>>(videos);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
