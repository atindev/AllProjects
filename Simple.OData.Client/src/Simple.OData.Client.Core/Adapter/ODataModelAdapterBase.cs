#pragma warning disable 1591

namespace Simple.OData.Client.WestInternal
{
    public abstract class ODataModelAdapterBase : IODataModelAdapter
    {
        public abstract AdapterVersion AdapterVersion { get; }

        public string ProtocolVersion { get; set; }
 
        public object Model { get; set; }
    }
}