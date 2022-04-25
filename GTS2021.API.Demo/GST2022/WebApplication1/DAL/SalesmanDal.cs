using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Model;

namespace WebApplication1.DAL
{
    public class SalesmanDal : ISalesmanDal
    {
        private readonly GstDbContext context;

        public SalesmanDal(GstDbContext context1)
        {
            context = context1;
        }

        public IQueryable<Salesman> GetAllSalesman()
        {
            try
            {
                return context.Salesmen.AsQueryable();
            }
            catch
            {
                return Enumerable.Empty<Salesman>().AsQueryable();
            }
        }

        public IQueryable<Salesman> GetAllSalesmanwithOrders()
        {
            try
            {
                return context.Salesmen.Include(x => x.Orders);
            }
            catch
            {
                return Enumerable.Empty<Salesman>().AsQueryable();
            }
        }

        public Salesman GetSpecificSalesman(int Id)
        {
            return context.Salesmen.Find(Id);
        }

        public bool DeleteSpecificSalesman(int Id)
        {
            Salesman sal = context.Salesmen.Find(Id);
            if (sal != null)
            {
                context.Salesmen.Remove(sal);
                context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public Salesman CreateSalesman(Salesman sal)
        {
            context.Salesmen.Add(sal);
            context.SaveChanges();
            return sal;
        }

        public Salesman updateSalesman(Salesman sal1)
        {
            context.SaveChanges();
            return sal1;
        }

        public IEnumerable<dynamic> Linq123()
        {
            IList<Salesman> sal = context.Salesmen.ToList();
            IList<Orders> ords = context.Orders.ToList();

            var data = (from s in sal
                        join ord in ords on s.Id equals ord.SalesmanId
                        group ord by s into grp
                        orderby grp.Key.Comission descending
                        select new
                        {
                            SalesmanId = grp.Key.Id,
                            grp.Key.Name,
                            grp.Key.City,
                            grp.Key.Comission,
                            TotalSalesVolume = grp.Count(),
                            AvgSalesVolume = grp.Average(x => x.Purchase_Amt),
                            Price = 0
                        }).Distinct();

            if (data?.Any() == true)
            {
                return data;
            }
            else
                return Enumerable.Empty<dynamic>();

            ////int[] p = new int[] { 0, 3, 4, 6, 7, 8 };
            ////p[0] = 6;
            ////int d = p.Sum();
        }
    }
}
