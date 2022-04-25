using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Template.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all Templates response object
        /// </summary>
        public List<Common.Models.Template> Templates { get; set; }
            = new List<Common.Models.Template>();

        
    }
}
