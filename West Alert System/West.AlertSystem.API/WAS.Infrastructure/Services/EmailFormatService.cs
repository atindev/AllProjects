using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class EmailFormatService : IEmailFormatService
    {
        private readonly IAzureStorageService _azureStorageService;

        public EmailFormatService(
            IAzureStorageService azureStorageService
            )
        {
            _azureStorageService = azureStorageService;
        }

        public async Task<string> FormatEmail(Object mailObject, string blobName)
        {
            var fileContent = await _azureStorageService.DowloadFileFromBlobStorage(blobName, "was-email-templates");

            foreach (PropertyInfo prop in mailObject.GetType().GetProperties())
            {
                fileContent = fileContent.Replace("{{"+prop.Name+"}}", prop.GetValue(mailObject, null).ToString());
            }

            return fileContent;
        }
    }
}

