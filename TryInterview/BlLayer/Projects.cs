using System.Linq;
using TryInterview.DalLayer;
using TryInterview.Models.DbModels;

namespace TryInterview.BlLayer
{
    public class Projects : IProjects
    {
        public readonly IProjectsDa projectsDa;

        public Projects(IProjectsDa projectsDa)
        {
            this.projectsDa = projectsDa;
        }

        public IQueryable<Project> GetProjects()
        {
            return projectsDa.GetProjects();
        }

        public IQueryable<dynamic> GetProjectsBillingData()
        {
            var projects = projectsDa.GetProjects();
            var projectsBllings = projectsDa.BillingData();

            var billingData = from proj in projects
                              join projBill in projectsBllings on new
                              {
                                  Amt = proj.BilledAmount,
                                  Type = 2//Credit
                              }
                              equals
                              new
                              {
                                  Amt = projBill.TransactionAmount,
                                  Type = projBill.TransactionType
                              }
                              select new
                              {
                                  proj.Id,
                                  proj.Name,
                                  proj.CreatedDateTime,
                                  proj.CustomerId,
                                  proj.BillingMode,
                                  proj.BilledAmount,
                                  BillId = projBill.Id,
                                  BillTransactionType = projBill.TransactionType,
                                  BillTransactionDate = projBill.TransactionDate,
                                  BillTransactionAmount = projBill.TransactionAmount
                              };

            return billingData;
        }
    }
}
