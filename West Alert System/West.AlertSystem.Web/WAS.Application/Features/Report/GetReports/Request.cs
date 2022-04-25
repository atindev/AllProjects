using MediatR;

namespace WAS.Application.Features.Report.GetReports
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int LocationId { get; set; }
    }
}
