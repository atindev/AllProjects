using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Enum;

namespace WAS.Application.Features.Notification.ApproveReject
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Notification id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Notification status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Notification approval level
        /// </summary>
        public ApprovalLevel ApprovalLevel { get; set; }

        /// <summary>
        /// Approved/Rejected by
        /// </summary>
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Approve/Reject comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Notification first level Action Timezone
        /// </summary>
        public string ApprovedTimeZone { get; set; }

        /// <summary>
        /// Notification second level approved Timezone
        /// </summary>
        public string FinalApprovalTimeZone { get; set; }
    }

}
