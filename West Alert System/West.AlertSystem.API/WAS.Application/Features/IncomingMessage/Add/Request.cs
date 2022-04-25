using MediatR;

namespace WAS.Application.Features.IncomingMessage.Add
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Incoming PhoneNumber
        /// </summary>
        public string FromPhone { get; set; }

        /// <summary>
        /// Incoming Audio/Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Is this Text
        /// </summary>
        public string IsText { get; set; }

        /// <summary>
        /// Is this WhatsApp
        /// </summary>
        public string IsWhatsApp { get; set; }

        /// <summary>
        /// Is this Audio
        /// </summary>
        public string IsVoice { get; set; }
    }
}
