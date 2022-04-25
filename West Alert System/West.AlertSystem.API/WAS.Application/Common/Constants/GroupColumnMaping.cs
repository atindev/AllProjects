
using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Constants
{
    public static class GroupColumnMaping
    {
        private static readonly IDictionary<string, string> ColumnMapping = new Dictionary<string, string>() {
                {"Location", "LocationId"},
                {"Shift", "ShiftId"},
                {"Department", "DepartmentId"},
                {"Email", "OfficialEmail"},
                {"Name", "FirstName"},
                {"LastName", "LastName"},
                {"Upn","Upn"},
                {"City","CityId" },
                {"State","StateId" },
                {"Country","CountryId" },
                {"JobTitle","JobTitle" },
                {"EmployeeType","EmployeeType" },
                {"CostCenter","CostCenter" },
                {"EmployeeGroup","EmployeeGroup"},
                {"SubscribedOn","CreatedDate"}
                };
        public static IDictionary<string, string> getColumnMapping()
        {
            return ColumnMapping;
        }
    }
}
