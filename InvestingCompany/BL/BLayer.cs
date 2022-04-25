using InvestingCompany.DAL;
using System.Collections.Generic;

namespace InvestingCompany.BL
{
    public class BLayer : IBLayer
    {
        private readonly IDalLayer dalLayer;

        public BLayer(IDalLayer _dalLayer)
        {
            dalLayer = _dalLayer;
        }

        public IEnumerable<dynamic> GetCompanyUserLevelDetails(int CompanyId)
        {
            return dalLayer.GetCompanyUserLevelDetails(CompanyId);
        }

        public IEnumerable<dynamic> GetCompanyLevelDetails()
        {
            return dalLayer.GetCompanyLevelDetails();
        }
    }
}
