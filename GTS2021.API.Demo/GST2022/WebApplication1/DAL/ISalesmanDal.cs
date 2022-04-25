using System.Collections.Generic;
using System.Linq;
using WebApplication1.Model;

namespace WebApplication1.DAL
{
    public interface ISalesmanDal
    {
        #region Public Methods

        Salesman CreateSalesman(Salesman sal);

        bool DeleteSpecificSalesman(int Id);

        IQueryable<Salesman> GetAllSalesman();

        IQueryable<Salesman> GetAllSalesmanwithOrders();

        Salesman GetSpecificSalesman(int Id);

        IEnumerable<dynamic> Linq123();

        Salesman updateSalesman(Salesman sal1);

        #endregion Public Methods
    }
}