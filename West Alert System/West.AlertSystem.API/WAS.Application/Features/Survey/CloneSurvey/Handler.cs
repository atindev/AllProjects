using MediatR;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GetSurvey = WAS.Application.Features.Survey.GetById;
using CreateUpdateSurvey = WAS.Application.Features.Survey.CreateUpdate;
using WAS.Application.Common.Constants;

namespace WAS.Application.Features.Survey.CloneSurvey
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {

            var currentSurveyBlobContent = await _mediator.Send(new GetSurvey.Request { Id = request.SurveyId });
            CreateUpdateSurvey.Request currentSurvey = JsonConvert.DeserializeObject<CreateUpdateSurvey.Request>
                                    (currentSurveyBlobContent?.SurveyContent);

            currentSurvey = ModifyCurrentSurveyContent(currentSurvey, request.EmailId);

            var createdSurveyResponse= await _mediator.Send(currentSurvey);

            return new Response { Success=createdSurveyResponse.Success, SurveyId = createdSurveyResponse.Id };
        }

        private CreateUpdateSurvey.Request ModifyCurrentSurveyContent(CreateUpdateSurvey.Request currentSurvey,string emailId)
        {
            
            StringBuilder temp = new StringBuilder();
            currentSurvey.Id = Guid.Empty;
            currentSurvey.CreatedBy = currentSurvey.ModifiedBy = emailId;
            currentSurvey.Subject = temp.Append(currentSurvey.Subject).Append(Constants.CLONE).ToString();
            foreach (var question in currentSurvey.Questions)
            {
                temp.Clear();
                question.Question = temp.Append(question.Question).Append(Constants.CLONE).ToString();
            }
            return currentSurvey;
        }
    }
}
