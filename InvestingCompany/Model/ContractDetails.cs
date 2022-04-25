using System;
using System.Collections.Generic;

namespace InvestingCompany.Model
{
    public class ContractDetails : BasicDetails
    {
        public DateTime StartedOn { get; set; }
        public DateTime EndsOn { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        ////public int UserId { get; set; }
        ////public virtual Users User { get; set; }
        public List<Users> UserIds { get; set; }
        public decimal UserAmount { get; set; }
    }
}
