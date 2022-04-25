using MediatR;

namespace WAS.Application.Features.Training.GetVideoById
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Video Id
        /// </summary>
        public int VideoId { get; set; }
    }
}
