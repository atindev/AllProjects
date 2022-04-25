using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Template
    {
        /// <summary>
        /// Primary key of Template
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the Template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Template Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// category Id
        /// </summary>
        public int CategoryId { get; set; }

    }
}
