using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Language
    {
        /// <summary>
        /// Primary key of Language
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Language Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// language code
        /// </summary>
        public string Code { get; set; }

    }
}
