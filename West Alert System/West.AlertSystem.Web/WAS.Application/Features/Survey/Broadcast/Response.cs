using System;

namespace WAS.Application.Features.Survey.Broadcast
{
    public class Response
    {
        /// <summary>
        /// broadcast survey unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Is broadcast success
        /// </summary>
        public bool Success { get; set; }
    }
}
