using Enterprise.Repository;
using MfgSystems.Model;
using System;
namespace Trials
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //MfgSystems.Model.WDOrderMixBatch
            IEnterpriseRepository<WDOrderMixBatch> obj = new EnterpriseSQLServerRepository<WDOrderMixBatch>(new SQLDb());

            obj.GetItemsAsync(c => c.Id != null);
        }
    }
}
