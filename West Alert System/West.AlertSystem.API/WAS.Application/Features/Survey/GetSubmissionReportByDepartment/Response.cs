using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.GetSubmissionReportByDepartment
{
    public class Response 
    {
        public List<CompletedSurveyReportByDepartment> CompletedSurveyByDepartment { get; set; }

        public List<PendingSurveySubmissionByDepartment> PendingSubmissionSurveyByDepartment { get; set; }
    }
}
