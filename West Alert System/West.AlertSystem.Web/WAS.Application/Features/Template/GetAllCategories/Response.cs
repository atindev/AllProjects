using System.Collections.Generic;
using WAS.Application.Common.Models;

namespace WAS.Application.Features.Template.GetAllCategories
{
    public class Response
    {

        /// <summary>
        /// List of Template Categories
        /// </summary>
        public List<TemplateCategory> TemplateCategories { get; set; }
    }
}
