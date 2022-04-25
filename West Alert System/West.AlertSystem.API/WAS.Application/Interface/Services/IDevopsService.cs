using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Features.Feedback;

namespace WAS.Application.Interface.Services
{
    public interface IDevopsService
    {
        Task<Response> SubmitUserFeedback(FeedbackResource feedbackResource);
    }
}
