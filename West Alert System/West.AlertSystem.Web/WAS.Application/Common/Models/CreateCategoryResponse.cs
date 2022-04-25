using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class CreateCategoryResponse
    {
        /// <summary>
        /// Category unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Is Category Create/update success
        /// </summary>
        public bool Success { get; set; }

    }
}
