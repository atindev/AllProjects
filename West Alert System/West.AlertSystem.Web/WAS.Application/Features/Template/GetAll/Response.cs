using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Template.GetAll
{
    public class Response
    {
        /// <summary>
        /// List of active Temlates
        /// </summary>
        public List<Common.Models.Template> Templates { get; set; }

        /// <summary>
        /// List of Template Categories
        /// </summary>
        public List<TemplateCategory> TemplateCategories { get; set; }
    }
}
