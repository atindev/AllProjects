using MediatR;

namespace WAS.Application.Features.Training.GetVideo
{
    public class Request : IRequest<Response>
    {
        public string Language { get; set; }
    }
}
