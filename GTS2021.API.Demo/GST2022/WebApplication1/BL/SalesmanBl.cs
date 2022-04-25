using System.Collections.Generic;
using System.Linq;
using WebApplication1.DAL;
using WebApplication1.Model;

namespace WebApplication1.BL
{
    public class SalesmanBl : ISalesmanBl
    {
        private readonly ISalesmanDal salesmanDal;

        public SalesmanBl(ISalesmanDal _salesmanDal)
        {
            salesmanDal = _salesmanDal;
        }

        public Salesman CreateSalesman(Salesman sal)
        {
            if (sal != null)
            {
                return salesmanDal.CreateSalesman(sal);
            }
            else
            {
                return null;
            }
        }

        public bool DeleteSpecificSalesman(int Id)
        {
            return salesmanDal.DeleteSpecificSalesman(Id);
        }

        public IQueryable<Salesman> GetAllSalesman()
        {
            return salesmanDal.GetAllSalesman();
        }

        public IQueryable<Salesman> GetAllSalesmanwithOrders()
        {
            return salesmanDal.GetAllSalesmanwithOrders();
        }

        public Salesman GetSpecificSalesman(int Id)
        {
            return salesmanDal.GetSpecificSalesman(Id);
        }

        public IEnumerable<dynamic> Linq123()
        {
            return salesmanDal.Linq123();
        }

        public Salesman updateSalesman(Salesman sal1)
        {
            Salesman sal = salesmanDal.GetSpecificSalesman(sal1.Id);
            if (sal != null)
            {
                sal.City = sal1.City;
                sal.Name = sal1.Name;
                sal.Comission = sal1.Comission;
                return salesmanDal.updateSalesman(sal);
            }
            else
            {
                return null;
            }
        }
    }
}
