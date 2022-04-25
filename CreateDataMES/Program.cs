using System;
using System.Globalization;
using System.Threading.Tasks;

namespace CreateDataMES
{
    public class Program
    {
        protected Program()
        {
        }

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CultureCheck();
            ///await CreateOrders();

            Console.WriteLine("\r\n\nFINISHED!");
            Console.ReadLine();
        }

        private static void CultureCheck()
        {
            ////var newCul = CultureInfo.CreateSpecificCulture("en");
            ////var newCul1 = CultureInfo.CreateSpecificCulture("fr");
            ////var newCul2 = CultureInfo.CreateSpecificCulture("pt");
            ////newCul.NumberFormat = new NumberFormatInfo()
            ////{
            ////    NumberDecimalSeparator = ",",
            ////    NumberDecimalDigits = 5,
            ////    NumberGroupSeparator = ".",
            ////    NumberGroupSizes = new int[] { 3, 2 }
            ////};

            //var indCult = new CultureInfo("en-in");

            decimal d = 123456789.1358658m;

            var systemCul = CultureInfo.InstalledUICulture;
            CultureInfo toBeUsed;

            switch (systemCul.TwoLetterISOLanguageName.ToLower())
            {
                case "en":
                    toBeUsed = new CultureInfo("en");
                    break;
                case "fr":
                    toBeUsed = new CultureInfo("fr");
                    break;
                case "pt":
                    toBeUsed = new CultureInfo("pt");
                    break;
                case "de":
                    toBeUsed = new CultureInfo("de");
                    break;
                default:
                    toBeUsed = new CultureInfo("en");
                    break;
            }

            toBeUsed.NumberFormat = systemCul.NumberFormat;


            Console.WriteLine(d.ToString("N", toBeUsed.NumberFormat));
            Console.WriteLine(d.ToString("N", CultureInfo.InvariantCulture));
            Console.WriteLine(d.ToString("N", CultureInfo.InstalledUICulture));

            ////12,34,56,789.13587
            ////12,34,56,789.14

            ////CultureInfo[] cis = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            ////var x = cis;//.Where(x => x.TwoLetterISOLanguageName == "pt");

            ////foreach (CultureInfo ci in x)
            ////{
            ////    Console.Write("{0,-15}", ci.Name);
            ////    Console.Write(" {0,-3}", ci.TwoLetterISOLanguageName);
            ////    Console.Write(" {0,-3}", ci.ThreeLetterISOLanguageName);
            ////    Console.Write(" {0,-3}", ci.ThreeLetterWindowsLanguageName);
            ////    Console.Write(" {0,-40}", ci.DisplayName);
            ////    Console.Write(" {0,-40}", ci.EnglishName);
            ////    Console.Write(" {0,-5}", ci.NumberFormat.NumberDecimalSeparator);
            ////    Console.WriteLine(" {0,-5}", ci.NumberFormat.NumberGroupSeparator);
            ////}
        }

        private static async Task CreateOrders()
        {
            string token = @"";

            CreateOrder o = new CreateOrder();

            ////Dev Create orders
            await o.CreateOrders(token, 221, 20);

            ////DVV create orders
            ////await o.CreateOrders(token, 241, 20, @"Order_DVV_Comp.json", "usendvvmes");

            ////EnumInputs enumInputs = new EnumInputs();
            ////await enumInputs.CreateEnums();

            ////PrecountCalc precountCalc = new PrecountCalc();
            ////precountCalc.Check();

            ////OdataDeleteCheck odataDelete = new OdataDeleteCheck();
            ////odataDelete.Check();
        }
    }
}
