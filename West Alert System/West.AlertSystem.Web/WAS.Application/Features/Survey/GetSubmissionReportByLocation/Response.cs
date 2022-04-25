using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Survey.GetSubmissionReportByLocation
{
    public class Response
    {
        public List<CompletedSurveyByLocation> CompletedSurveyByLocation { get; set; }

        public List<PendingSurveySubmissionByLocation> PendingSubmissionSurveyByLocation { get; set; }
    }
}
