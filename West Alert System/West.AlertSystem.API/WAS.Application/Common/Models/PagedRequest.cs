using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class PagedRequest 
    {
        /// <summary>
        /// Current Page number
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Number Of rows in a page
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// complete or Paged
        /// </summary>
        public string PageType { get; set; }
    }
}
