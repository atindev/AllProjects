using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.GetSubjectByBroadcastId
{
    public class Response  
    {
        /// <summary>
        /// broadcasted survey subject
        /// </summary>
         public string Subject { get; set; }
    }
}
