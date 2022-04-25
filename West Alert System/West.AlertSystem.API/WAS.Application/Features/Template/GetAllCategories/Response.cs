using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Template.GetAllCategories
{
    public class Response
    {
        /// <summary>
        /// Get all Templates response object
        /// </summary>
        public List<Common.Models.TemplateCategory> TemplateCategories { get; set; }
            = new List<Common.Models.TemplateCategory>();

        
    }
}
