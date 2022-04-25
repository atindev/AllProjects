using System.Collections.Generic;

namespace InvestingCompany.DAL
{
    public interface IDalLayer
    {
        IEnumerable<dynamic> GetCompanyLevelDetails();
        IEnumerable<dynamic> GetCompanyUserLevelDetails(int CompanyId);
    }
}