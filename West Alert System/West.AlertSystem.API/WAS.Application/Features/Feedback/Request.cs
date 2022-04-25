using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Feedback
{
  public class Request:IRequest<Response>
    {
        /// <summary>
        /// Feedback id
        /// </summary>
        public int FeedbackID { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User email address
        /// </summary>
        public string UserEmailID { get; set; }

        /// <summary>
        /// Feed back title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///  User description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// User feed back picture base convertor
        /// </summary>
        public byte[] PictureBase64 { get; set; }

        /// <summary>
        /// user feed back picture name
        /// </summary>
        public string PictureFileName { get; set; }

        /// <summary>
        ///Convert to base 64 format to  user picture
        /// </summary>
        public string Base64String { get; set; }

        /// <summary>
        /// screenshot need to include or not
        /// </summary>
        public bool isScreenshotRequired { get; set; }
    }
}
