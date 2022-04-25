using System.Collections.Generic;

namespace TryInterview.Models.DbModels
{
    public class Company : EntityBaseDbModel
    {
        public ICollection<Customer> Customers { get; set; }
    }
}
