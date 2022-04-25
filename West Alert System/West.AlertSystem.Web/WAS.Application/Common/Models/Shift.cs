using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class Shift
    {
        /// <summary>
        /// Primary key of shift
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the shift
        /// </summary>
        public string Name { get; set; }
    }
}
