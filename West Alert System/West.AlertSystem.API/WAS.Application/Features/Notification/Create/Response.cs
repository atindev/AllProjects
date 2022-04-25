using System;

namespace WAS.Application.Features.Notification.Create
{
    public class Response
    {
        /// <summary>
        /// Event unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Is subscription success
        /// </summary>
        public bool Success { get; set; }
    }
}
