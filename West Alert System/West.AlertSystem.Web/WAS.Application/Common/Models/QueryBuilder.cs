using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class QueryBuilder : PagedRequest
    {
        /// <summary>
        /// query Condition
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        /// Query rule from query builder
        /// </summary>
        public List<Rule> rules { get; set; }

    }
}