using System;
using System.Threading.Tasks;
using West.Kiosk.Core.ScaleLib;
using West.Kiosk.Core.ScaleLib.Implementations;
using West.Kiosk.Core.ScaleLib.ScaleLibrary;

namespace ScaleTryConsole
{
    class Program
    {
        protected Program() { }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ScaleObject scaleobj = new ScaleObject()
            {
                Id = "",
                MfgEquipmentId = "5ECC3E23-CFD6-4058-975D-D42A6ACD8A18",
                ScalehostName = "10.69.40.72",
                Precision = 3,
                WritePort = 4305,
                ReadPort = 4305,
                UOM = "KG",
                ScaleLibrary = nameof(MettlerToledoV2),
                cmdTare = "T",
                cmdRead = "S",
                cmdResetClear = "@",
                cmdZero = "Z"
            };

            AbsNetworkScale scale = new NetworkScale(scaleobj);
            ScaleResults data = await scale.GetWeight();
            Console.WriteLine(data.ScaleGrossText);
        }
    }
}
