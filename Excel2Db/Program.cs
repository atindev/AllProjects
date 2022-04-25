#define Certificate
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Excel2Db
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ExcelTry excelTry = new ExcelTry();
            excelTry.ReadExcel();
        }

        static void MainAsync(string[] args)
        {
            IConfiguration config = null;
            Console.WriteLine("Hello World!");
            try
            {
                using (var store = new X509Store(StoreLocation.CurrentUser))
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("settings.json", optional: true);

                    config = builder.Build();
                    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, config["AzureADCertThumbprint"], false);
                    builder.AddAzureKeyVault($"https://{config["KeyVaultName"]}.vault.azure.net/", config["AzureADApplicationId"], certs.OfType<X509Certificate2>().Single());

                    config = builder.Build();

                    foreach (var item in config.GetChildren())
                    {
                        Console.WriteLine(item.Key + ": " + item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
