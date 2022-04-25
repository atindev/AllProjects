using System;
using System.Collections.Generic;
using System.Text;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface ITriangularValidationService
    {
        List<string> GetStaticRandomNames();

        List<string> GetSelectedRandomNames(List<string> inputList,int count, bool includeStaticNames = true);

        List<string> InsertToRandomPosition(List<string> inputList, string element);
    }
}
