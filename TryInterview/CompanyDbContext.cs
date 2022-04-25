using Microsoft.EntityFrameworkCore;
using TryInterview.Models.DbModels;

namespace TryInterview
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Company> Company { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<BillingData> BillingData { get; set; }
    }
}
