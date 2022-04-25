using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GTS2021.API.Demo.Models
{
    public class UserTask
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public int status { get; set; } = 0;

        public int UserId { get; set; }

        public virtual User CreatedBy { get; set; }

        [NotMapped]
        public string Deadline
        {
            get
            {
                return (DateTime.Now - DueDate).TotalDays.ToString("F0") + " Days";
            }
        }

        [NotMapped]
        public string Progress
        {
            get
            {
                return ((TaskStatus)status).ToString();
            }
        }
    }
}
