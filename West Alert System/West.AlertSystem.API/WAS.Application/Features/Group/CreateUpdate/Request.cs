using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WAS.Application.Features.Group.CreateUpdate
{
    public class Request : IRequest<Response>
    {
        /// <summary>
        /// Group unique identifier
        /// </summary>
        public List<int> Id { get; set; }


        /// <summary>
        /// Group name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// User who last modified the record
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Is this only accessable to owner
        /// </summary>
        public bool IsPrivate { get; set; }

        /// <summary>
        /// Is this acccessable to members of the group
        /// </summary>
        public bool IsAccessToAdmins { get; set; }

    }

}
