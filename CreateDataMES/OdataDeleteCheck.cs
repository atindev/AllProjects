using System;
using System.Threading.Tasks;

namespace CreateDataMES
{
    public class OdataDeleteCheck
    {
        public void Check()
        {
            CheckDelete().GetAwaiter().GetResult();
        }

        public async Task CheckDelete()
        {
            Console.WriteLine("Delete Start");

            var oDataClient = new Simple.OData.Client.ODataClient("https://localhost:44321/odata");
            var id = new Guid("24537D4A-806D-4047-B415-12AD25A2DCFB");
            await oDataClient.For("MfgScale")
                .Key(id)
                .QueryOptions("DeleteRecord=true")
                .DeleteEntryAsync();

            Console.WriteLine("Records deleted1: ");

            await oDataClient.For("MfgScale")
                .Key(id)
                .QueryOptions("DeleteRecord=true")
                .DeleteEntriesAsync();

            Console.WriteLine("Records deleted2: ");
        }
    }
}
