using System;
using System.Collections.Generic;
using System.Text;
using Syncfusion.EJ2.Base;


namespace WAS.Web.Models
{
    public class FilterRequest: DataManagerRequest
    {
        /// <summary>
        /// Filter parameter from Grid
        /// </summary>
        public Dictionary<string, string> Params { get; set; }
         
    }
}
