using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WAS.Domain.Enum;

namespace WAS.Application.Features.Notification.GetByStatus
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Request status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// email id of the person
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// the person is only private approver not a first or second level approver
        /// </summary>
        public bool IsOnlyPrivateApprover { get; set; } = false;

        /// <summary>
        /// is having access to all groups
        /// </summary>
        public bool IsGlobalAdmin { get; set; }

        /// <summary>
        /// having approver and communication team 
        /// </summary>
        public bool HavingBothApprovalLevel { get; set; }
    }
}
