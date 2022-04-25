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
using WAS.Application.Interface.Services;
using Microsoft.Identity.Client;

namespace WAS.Application.Features.Survey.GetAllSharedPeopleNamesById
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
                List<string> PeopleEmailIds = new List<string>();
                List<string> PeopleName = new List<string>();
                PeopleEmailIds.AddRange(await _context.SurveyDetailShare.Where(x => x.BroadcastId == request.BroadcastId).Select(x => x.OfficialMail).ToListAsync(cancellationToken));
                var distinctPeopleEmailId = PeopleEmailIds.Distinct().ToList();
                foreach(var item in distinctPeopleEmailId)
                {

                    var firstName = _context.Subscriptions.IgnoreQueryFilters().FirstOrDefault(x => x.OfficialEmail == item).FirstName;
                    var lastName = _context.Subscriptions.IgnoreQueryFilters().FirstOrDefault(x => x.OfficialEmail == item).LastName;
                    var fullName = firstName + " " + lastName;
                    PeopleName.Add(fullName);
                }
                return new Response
                {
                    PeopleName = PeopleName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }
    }
}
