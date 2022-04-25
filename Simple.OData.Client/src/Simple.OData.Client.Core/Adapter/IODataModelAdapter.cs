using System;

#pragma warning disable 1591

namespace Simple.OData.Client.WestInternal
{
    public interface IODataModelAdapter
    {
        AdapterVersion AdapterVersion { get; }
        string ProtocolVersion { get; set; }
        object Model { get; set;  }
    }
}
