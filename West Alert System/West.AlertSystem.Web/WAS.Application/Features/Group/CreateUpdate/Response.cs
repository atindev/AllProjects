using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WAS.Application.Features.Groups.CreateUpdate
{
    public class Response
    {
        /// <summary>
        /// Group unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Is Group Create/update success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Is group name already exist
        /// </summary>
        public bool IsNameExist { get; set; }
    }
}

