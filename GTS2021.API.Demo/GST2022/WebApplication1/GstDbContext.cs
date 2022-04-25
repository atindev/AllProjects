using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Model;

namespace WebApplication1
{
    public class GstDbContext : DbContext
    {
        public GstDbContext([NotNullAttribute] DbContextOptions options) : base(options) { }

        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Orders> Orders { get; set; }
    }
}