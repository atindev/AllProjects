using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Shift.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all shift response object
        /// </summary>
        public List<Shift> Shifts { get; set; }
            = new List<Shift>();
    }
}
