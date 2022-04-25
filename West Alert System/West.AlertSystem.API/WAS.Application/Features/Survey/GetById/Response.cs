using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.GetById
{
    public class Response
    {
        /// <summary>
        /// for json content
        /// </summary>
        public string SurveyContent { get; set; }
    }
}
