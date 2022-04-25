using MediatR;

namespace WAS.Application.Features.IncomingMessage.GetAll
{
    public class Request : IRequest<Response>
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
        /// Message filter
        /// </summary>
        public string MessageFilter { get; set; }

        /// <summary>
        /// complete page or particular page
        /// </summary>
        public string PageType { get; set; } = "Complete";
    }
}
