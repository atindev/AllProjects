using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }
        public decimal Purchase_Amt { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comments { get; set; }
        public int Rating { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int SalesmanId { get; set; }
        public virtual Salesman Salesman { get; set; }
    }
}
