using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class Template : Entity
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
        /// Template Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Template Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Category Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Template Category
        /// </summary>
        public virtual TemplateCategory TemplateCategory { get; set; }

    }
}
