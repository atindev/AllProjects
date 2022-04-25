using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GTS2021.API.Demo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailId { get; set; }

        public ICollection<UserTask> userTasks { get; set; }
    }
}
