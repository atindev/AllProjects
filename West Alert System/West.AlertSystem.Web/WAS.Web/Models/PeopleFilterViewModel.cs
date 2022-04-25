using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WAS.Web.Models
{
    public class PeopleFilterViewModel
    {
        public string[] AndOrCondition { get; set; }

        public Application.Common.Enum.FilterType[] FilterTypes { get; set; }

        public string[] FilterConditions { get; set; }

        public string[] FilterValues { get; set; }
    }
}
