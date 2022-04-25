using Simple.OData.Client;
using System.Linq;
using System.Threading.Tasks;
using West.Manufacturing.Common.Util;
using West.Manufacturing.Model;

namespace CreateDataMES
{
    class OdataCreate
    {
        public async Task CreateOrders()
        {
            string OdataUrl = "";
            var odataClient = new ODataClient(OdataUrl);
            var data = await odataClient.For("MfgOrder").Filter("Id eq 49EC65CD-595E-4A5C-9CF2-8BD904E3939F").FindEntryAsync();

            _ = ODataUtil.MapClientDataToEntity<MfgOrder>(data).First();
        }
    }
}
