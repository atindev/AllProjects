using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Subscription.GetByMail
{
    public class Response
    {
        /// <summary>
        /// Subscription
        /// </summary>
        public Common.Models.Subscription Subscription { get; set; }

    }
}
