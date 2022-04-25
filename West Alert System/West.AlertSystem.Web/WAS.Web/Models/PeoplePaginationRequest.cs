using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.EJ2.Base;
using WAS.Application.Common.Models;

namespace WAS.Web.Models
{
    public class PeoplePaginationRequest : DataManagerRequest
    {
        /// <summary>
        /// Filter parameter from Grid
        /// </summary>
        public Dictionary<string, QueryBuilderRequest> Params { get; set; }
    }

    public class QueryBuilderRequest
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
