using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Group.GetByIds
{
    public class Response
    {
        /// <summary>
        /// Get all subscriptions response object
        /// </summary>
        public List<Common.Models.Group> Group { get; set; }
    }
}
