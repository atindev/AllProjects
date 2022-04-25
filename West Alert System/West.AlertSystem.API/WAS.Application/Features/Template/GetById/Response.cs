using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Template.GetById
{
    public class Response
    {
        /// <summary>
        /// for json content
        /// </summary>
        public string TemplateContent { get; set; }
    }
}
