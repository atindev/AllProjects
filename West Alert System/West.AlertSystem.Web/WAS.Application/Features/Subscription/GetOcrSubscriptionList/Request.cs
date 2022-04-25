using MediatR;

namespace WAS.Application.Features.Subscription.GetOcrSubscriptionList
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Admin OfficialEmail
        /// </summary>
        public string AdminOfficialEmail { get; set; }

        /// <summary>
        /// Location ID
        /// </summary>
        public int? LocationId { get; set; }
    }
}
