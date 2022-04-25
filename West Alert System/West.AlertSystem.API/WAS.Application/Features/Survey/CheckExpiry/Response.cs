using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Survey.CheckExpiry
{
    public class Response
    {
        /// <summary>
        /// survey status
        /// </summary>
        public bool IsActive { get; set; }
    }
}
