using System.ComponentModel.DataAnnotations;

namespace ApiTsts
{
    public class TestApi2
    {
        [Key]
        public int Id { get; set; }
        public int Phone { get; set; }
        public int TestApiId { get; set; }
        public TestApi TestApi { get; set; }
    }
}
