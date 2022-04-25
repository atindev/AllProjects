using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WAS.Application.Common.Exceptions;
using WAS.Application.Interface;
using WAS.Domain.Entities;
using WAS.Application.Interface.Services;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel;
using WAS.Application.Common.Models;
using WAS.Application.Common.Constants;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Subscription.GetAll
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IWasContext _context;
        private readonly ILogger<Handler> _logger;
        private readonly IMapper _mapper;
        private readonly IGenerateExpression _generateExpression;
        int Count = 0;

        public Handler(
            IWasContext context,
            ILogger<Handler> logger,
            IMapper mapper,
            IGenerateExpression generateExpression
            )
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _generateExpression = generateExpression;
            Count = 0;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            List<Subscription> responseSubscriptionResult = new List<Subscription>();
            try
            { 
                if(request.PageType == "Paged")
                {
                    request.RowCount = (request.RowCount == 0) ? 10 : request.RowCount;
                    if (request.condition == null || request.rules == null || request.rules.Count == 0)
                    {
                        Count = _context.Subscriptions.Count();
                        var subscriptions = await _context.Subscriptions
                                  .Include(i => i.Location)
                                  .Include(i => i.Shift)
                                  .OrderByDescending(i => i.CreatedDate)
                                  .Skip(request.PageIndex).Take(request.RowCount)
                                  .ToListAsync(cancellationToken);
                        responseSubscriptionResult = _mapper.Map<List<Subscription>>(subscriptions);
                    }
                    else
                        responseSubscriptionResult = await executeExpression(request,cancellationToken);
                }

                else
                {
                    var subscriptions = await _context.Subscriptions
                      .Include(i => i.Location)
                        .ThenInclude(i => i.City)
                         .ThenInclude(i => i.State)
                           .ThenInclude(i => i.Country)
                      .Include(i => i.Shift)
                      .OrderBy(i => i.FirstName)
                      .ToListAsync(cancellationToken);

                    var responseSubscription = _mapper.Map<List<SubscriptionforQuery>>(subscriptions);
                    Count = responseSubscription.Count;
                    responseSubscriptionResult = _mapper.Map<List<Subscription>>(responseSubscription);
                }
                 
                var response = new Response()
                {
                    Subscriptions = responseSubscriptionResult,
                    Count = Count
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new BadRequestException(ex.Message, ex);
            }
        }

        public async Task<List<Subscription>> executeExpression(Request request, CancellationToken cancellationToken)
        {
            List<Subscription> responseSubscriptionResult = new List<Subscription>();
            ParameterExpression paramExpr = Expression.Parameter(typeof(SubscriptionforQuery));
            Expression expressionMain = null;
            Rule currentRule = new Rule();
            currentRule.condition = request.condition;
            currentRule.rules = request.rules;
            expressionMain = _generateExpression.BuildExpression(paramExpr, currentRule, GroupColumnMaping.getColumnMapping());
            if (expressionMain != null)
            {
                var subscriptions = await _context.Subscriptions
                                     .Include(i => i.Location)
                                       .ThenInclude(i => i.City)
                                        .ThenInclude(i => i.State)
                                          .ThenInclude(i => i.Country)
                                     .Include(i => i.Shift)
                                     .OrderByDescending(i => i.CreatedDate)
                                     .ToListAsync(cancellationToken);
                var responseSubscription = _mapper.Map<List<SubscriptionforQuery>>(subscriptions);
                var lambda = Expression.Lambda<Func<SubscriptionforQuery, bool>>(expressionMain, paramExpr);
                var compiledLambda = lambda.Compile();
                IQueryable<SubscriptionforQuery> responseSubscriptionSource = responseSubscription.AsQueryable();
                Count = responseSubscriptionSource
                   .Where(compiledLambda).Count();
                var queryResult = responseSubscriptionSource
                    .Where(compiledLambda)
                    .Skip(request.PageIndex).Take(request.RowCount)
                    .ToList();
                responseSubscriptionResult = _mapper.Map<List<Subscription>>(queryResult);
            }
            else
            {
                Count = _context.Subscriptions.Count();
                var subscriptions = await _context.Subscriptions
                          .Include(i => i.Location)
                          .Include(i => i.Shift)
                          .OrderByDescending(i => i.CreatedDate)
                          .Skip(request.PageIndex).Take(request.RowCount)
                          .ToListAsync(cancellationToken);
                responseSubscriptionResult = _mapper.Map<List<Subscription>>(subscriptions);
            }
            return responseSubscriptionResult;
        }

    }
}
