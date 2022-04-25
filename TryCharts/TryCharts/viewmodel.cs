using ScaleTryConsole;
using System.Threading.Tasks;
using West.Kiosk.Core.ScaleLib;
using West.Kiosk.Core.ScaleLib.Implementations;
using West.Kiosk.Core.ScaleLib.ScaleLibrary;

namespace TryCharts
{
    public class ViewModel
    {
        ////public ObservableCollection<ChartModel> ChartValueList { get; set; }
        ////public ObservableCollection<ChartModel> ChartValueList1 { get; set; }
        public ScaleObject scaleobj { get; set; }

        private readonly AbsNetworkScale scale;

        public ViewModel()
        {
            ////ChartValueList1 = new ObservableCollection<ChartModel>()
            ////{
            ////    new ChartModel(){StatusDescription = "A", StatusTimeSpan=1, PUName="PU1"},
            ////    new ChartModel(){StatusDescription = "B", StatusTimeSpan=3, PUName="PU1"},
            ////    new ChartModel(){StatusDescription = "B", StatusTimeSpan=5, PUName="PU1"},
            ////};

            ////ChartValueList = new ObservableCollection<ChartModel>(
            ////    (from item in ChartValueList1
            ////     group item by new { item.StatusDescription } into a
            ////     select new ChartModel
            ////     {
            ////         StatusDescription = a.Key.StatusDescription,
            ////         StatusTimeSpan = a.Sum(x => x.StatusTimeSpan)
            ////     }).ToList()
            ////);

            scaleobj = new ScaleObject()
            {
                Id = "",
                MfgEquipmentId = "5ECC3E23-CFD6-4058-975D-D42A6ACD8A18",
                ScalehostName = "10.69.40.72",
                Precision = 3,
                WritePort = 4305,
                ReadPort = 4305,
                UOM = "G",
                ScaleLibrary = nameof(MettlerToledoV2),
                cmdTare = "T",
                cmdRead = "S",
                cmdResetClear = "@",
                cmdZero = "Z"
            };

            scale = new NetworkScale(scaleobj);
        }

        public async Task<ScaleResults> GetWeight()
        {
            return await scale.GetWeight();
        }

        public async Task<ScaleResults> GetTare()
        {
            return await scale.GetTare();
        }

        ////public void FormChart(SfChart sfChart)
        ////{
        ////    foreach (var item in ChartValueList1)
        ////    {
        ////        StackingBarSeries stackBarSeries = new StackingBarSeries()
        ////        {
        ////            ItemsSource = new ObservableCollection<ChartModel>() { item },
        ////            XBindingPath = "PUName",
        ////            YBindingPath = "StatusTimeSpan",
        ////            StrokeWidth = 2,
        ////            EnableAnimation = true,
        ////            LegendIcon = ChartLegendIcon.Rectangle,
        ////        };
        ////        sfChart.Series.Add(stackBarSeries);
        ////    }
        ////}
    }
}
