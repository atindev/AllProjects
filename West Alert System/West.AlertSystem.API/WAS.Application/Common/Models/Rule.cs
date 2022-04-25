using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Rule
    {
        /// <summary>
        /// filter label
        /// </summary>
        public string label { get; set; }

        /// <summary>
        /// filter field
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// selected operator
        /// </summary>
        public string @operator { get; set; }

        /// <summary>
        /// selected type
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// selected value
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// selected condition
        /// </summary>
        public string condition { get; set; }

        /// <summary>
        /// selected rule
        /// </summary>
        public List<Rule> rules { get; set; }
    }

}