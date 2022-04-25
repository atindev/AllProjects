using System.Collections.Generic;

namespace WAS.Application.Features.Language.GetAll
{
    public class Response
    {
        /// <summary>
        /// Get all Language response object
        /// </summary>
        public List<Common.Models.Language> Languages { get; set; }
            = new List<Common.Models.Language>();
    }
}
