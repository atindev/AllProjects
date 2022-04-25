using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Group.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all groups response object
        /// </summary>
        public List<Group> Groups { get; set; }
            = new List<Group>();

        /// <summary>
        /// Total Number of Groups
        /// </summary>
        public int Count { get; set; }
    }
}
