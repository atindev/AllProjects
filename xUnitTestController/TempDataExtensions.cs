using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace xUnitTestController
{
    public static class TempDataExtensions
    {
        public static void Put(this ITempDataDictionary TempData, string key, object data)
        {
            TempData[key] = JsonConvert.SerializeObject(data);
        }
    }
}
