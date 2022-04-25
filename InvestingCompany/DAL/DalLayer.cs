using InvestingCompany.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InvestingCompany.DAL
{
    public class DalLayer : IDalLayer
    {
        private List<Users> UserData { get; set; }
        private List<Company> CompanyData { get; set; }
        private List<ContractDetails> ContractsData { get; set; }

        public DalLayer()
        {
            UserData = new List<Users>()
            {
                new Users()
                {
                    Id = 1,
                    CreatedOn= DateTime.Now,
                    MaxLimit=10000,
                    Name="Atin"
                },
                new Users()
                {
                    Id = 2,
                    CreatedOn= DateTime.Now,
                    MaxLimit=15000,
                    Name="Yatin"
                },
                new Users()
                {
                    Id = 3,
                    CreatedOn= DateTime.Now,
                    MaxLimit=20000,
                    Name="Amit"
                }
            };

            CompanyData = new List<Company>()
            {
                new Company()
                {
                    Id = 1,
                    CreatedOn= DateTime.Now,
                    Name="HP",
                    CompanyAddress="Delhi"
                },
                new Company()
                {
                    Id = 2,
                    CreatedOn= DateTime.Now,
                    Name="IBM",
                    CompanyAddress="Mumbai"
                },
                new Company()
                {
                    Id = 3,
                    CreatedOn= DateTime.Now,
                    Name="Accenture",
                    CompanyAddress="Bangalore"
                }
            };

            ContractsData = new List<ContractDetails>()
            {
                new ContractDetails()
                {
                    Id = 1,
                    CreatedOn= DateTime.Now,
                    Name = string.Empty,
                    CompanyId = 1,
                    UserId = 1,
                    StartedOn = DateTime.Now,
                    EndsOn = DateTime.Now.AddYears(3),
                    UserAmount= 1000
        },
                new ContractDetails()
        {
            Id = 2,
                    CreatedOn = DateTime.Now,
                    Name = string.Empty,
                    CompanyId = 1,
                    UserId = 2,
                    StartedOn = DateTime.Now,
                    EndsOn = DateTime.Now.AddYears(3),
                    UserAmount= 1500
                },
                new ContractDetails()
        {
            Id = 3,
                    CreatedOn = DateTime.Now,
                    Name = string.Empty,
                    CompanyId = 1,
                    UserId = 3,
                    StartedOn = DateTime.Now,
                    EndsOn = DateTime.Now.AddYears(3),
                    UserAmount= 1500
                },
                new ContractDetails()
        {
            Id = 4,
                    CreatedOn = DateTime.Now,
                    Name = string.Empty,
                    CompanyId = 2,
                    UserId = 2,
                    StartedOn = DateTime.Now,
                    EndsOn = DateTime.Now.AddYears(3),
                    UserAmount= 1500
                },
                new ContractDetails()
        {
            Id = 5,
                    CreatedOn = DateTime.Now,
                    Name = string.Empty,
                    CompanyId = 3,
                    UserId = 3,
                    StartedOn = DateTime.Now,
                    EndsOn = DateTime.Now.AddYears(3),
                    UserAmount= 1500
                }
    };

        }

        public IEnumerable<dynamic> GetCompanyLevelDetails()
        {
            var data = from contract in ContractsData
                       join user in UserData on contract.UserId equals user.Id
                       join company in CompanyData on contract.CompanyId equals company.Id
                       group contract by new { contract.Name, company.Id } into g
                       select new
                       {
                           Name = g.Key.Name,
                           CompanyId = g.Key.Id,
                           Allocation = g.Sum(x => x.UserAmount)
                       };

            return data;
        }

        public IEnumerable<dynamic> GetCompanyUserLevelDetails(int CompanyId)
        {
            var data = from contract in ContractsData
                       join user in UserData on contract.UserId equals user.Id
                       join company in CompanyData on contract.CompanyId equals company.Id
                       where company.Id == CompanyId
                       select new
                       {
                           UserId = contract.UserId,
                           Name = user.Name,
                           CompanyId = contract.CompanyId,
                           CompanyName = company.Name,
                           UserContribution = contract.UserAmount,
                           StartedOn = contract.StartedOn,
                           EndsOn = contract.EndsOn
                       };

            return data;
        }
    }
}
