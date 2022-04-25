using System.ComponentModel.DataAnnotations;

namespace ApiTsts
{
    public class TestApi
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public int pincode { get; set; }
    }
}
