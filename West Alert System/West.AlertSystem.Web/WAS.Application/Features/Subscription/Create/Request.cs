using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Features.Subscription.Create
{
    public class Request :Common.Models.SubscriptionDetails, IRequest<Response>
    {
        /// <summary>
        /// Subscription Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Location Id
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Shift Id
        /// </summary>
        public int? ShiftId { get; set; }

        /// <summary>
        /// Subscription Mode
        /// </summary>
        public string SubscriptionMode { get; set; }

        /// <summary>
        /// Location name
        /// </summary>
        public string OfficeLocation { get; set; }

        /// <summary>
        /// Department
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// EmployeeId
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// JobTitle
        /// </summary>
        public string JobTitle { get; set; }        

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// PostalCode
        /// </summary>
        public int? PostalCode { get; set; }

        /// <summary>
        /// Upn
        /// </summary>
        public string Upn { get; set; }

        /// <summary>
        /// Employee type
        /// </summary>
        public string EmployeeType { get; set; }

        /// <summary>
        /// Employee group
        /// </summary>
        public string EmployeeGroup { get; set; }

        /// <summary>
        /// Cost center
        /// </summary>
        public string CostCenter { get; set; }


        /// <summary>
        /// SubscriptionReviewId for OCR Subscription
        /// </summary>
        public Guid? SubscriptionReviewId { get; set; }
    }
}
