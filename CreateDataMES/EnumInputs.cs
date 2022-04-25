using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using West.Manufacturing.Common.Util.LanguageSupport;
using West.Manufacturing.Model;

namespace CreateDataMES
{
    public class EnumInputs
    {
        private readonly Simple.OData.Client.ODataClient Client = new Simple.OData.Client.ODataClient(string.Join("/", "https://usendevmes.westpharma.net:30001/odatarepositoryapi", "odata/"));

        public async Task CreateEnums()
        {
            List<Task> tasks = new List<Task>();
            string[] list = Enum.GetNames(typeof(Error));
            list = list.Concat(Enum.GetNames(typeof(Information))).ToArray();
            list = list.Concat(Enum.GetNames(typeof(Messages))).ToArray();

            foreach (string item in list)
            {
                tasks.Add(CheckandInsert(item));
            }
            await Task.WhenAll(tasks);
        }

        private async Task CheckandInsert(string item)
        {
            int chk = await Client.For<MfgMessage>()
                .Filter(x => x.MessageCode == item)
                .Count()
                .FindScalarAsync<int>();


            if (chk == 0)
            {
                await ItemInsert(new MfgMessage()
                {
                    MessageCode = item,
                    CreatedByUser = "SYSTEM",
                    MessageDEscription = "PLEASE INSERT"
                });
            }
        }

        private async Task ItemInsert(MfgMessage item)
        {
            await Client.For<MfgMessage>()
                .Set(new
                {
                    item.CreatedByUser,
                    item.MessageCode,
                    item.MessageDEscription
                })
                .InsertEntryAsync(false);
        }
    }
}
