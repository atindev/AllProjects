using MfgSystems.Model;
using Microsoft.EntityFrameworkCore;

namespace Trials
{
    class SQLDb : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(
        //            ConfigurationManager.AppSettings["MFGSYSDB"],
        //            opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds))
        //        ;
        //}
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDbContext<BloggingContext>(options =>
        //        options.UseSqlServer(ConfigurationManager.AppSettings["MFGSYSDB"]));
        //}

        public DbSet<WDOrderMixBatch> WDOrderMixBatch { get; set; }

    }
}
