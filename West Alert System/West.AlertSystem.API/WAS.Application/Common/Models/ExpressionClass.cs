using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WAS.Application.Features.Subscription.GetAll;

namespace WAS.Application.Common.Models
{
    public class ExpressionClass
    {
        /// <summary>
        /// Query rule 
        /// </summary>
        public Rule rules { get; set; }

        /// <summary>
        /// generated Expression for the rule
        /// </summary>
        public Expression expression { get; set; }
    }
}
