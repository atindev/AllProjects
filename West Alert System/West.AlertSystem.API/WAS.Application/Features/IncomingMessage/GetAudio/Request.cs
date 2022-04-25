using MediatR;

namespace WAS.Application.Features.IncomingMessage.GetAudio
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Audio Path
        /// </summary>
        public string Path { get; set; }
    }
}
