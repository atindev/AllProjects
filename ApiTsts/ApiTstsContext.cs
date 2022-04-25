using Microsoft.EntityFrameworkCore;

namespace ApiTsts
{
    public class ApiTstsContext : DbContext
    {
        public ApiTstsContext(DbContextOptions<ApiTstsContext> options) : base(options)
        {
        }

        public DbSet<TestApi> TestApi { get; set; }
        public DbSet<TestApi2> TestApi2 { get; set; }
    }
}