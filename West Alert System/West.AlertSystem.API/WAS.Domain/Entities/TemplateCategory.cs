using System;
using System.Collections.Generic;
using System.Text;
using WAS.Domain.Common;

namespace WAS.Domain.Entities
{
    public class TemplateCategory : Entity
    {
        /// <summary>
        /// Category Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of Template
        /// </summary>
        public virtual ICollection<Template> Templates { get; set; }
    }
}
