using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Salesman
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public decimal Comission { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
