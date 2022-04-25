using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Settings;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly AzureStorageSettings _azureStorageSettings;

        public AzureStorageService(IOptions<AzureStorageSettings> azureStorageSettings)
        {
            _azureStorageSettings = azureStorageSettings.Value;
        }

        public async Task AddMessageToStorageQueue(object messageToQueue, string queueName)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(_azureStorageSettings.StorageConnectionString);
            // Create a queue client for interacting with the queue service
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = queueClient.GetQueueReference(queueName);
            // Insert a message into the queue using the AddMessage method. 
            await queue.AddMessageAsync(new CloudQueueMessage(JsonConvert.SerializeObject(messageToQueue)));
        }

        public async Task<Uri> UploadFileToBlobStorage(string blobName, Byte[] emailAttachment)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(_azureStorageSettings.StorageConnectionString);
            // Create a blob client for interacting with the blob service
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(_azureStorageSettings.EmailAttachmentContainer);
            await blobContainer.CreateIfNotExistsAsync();

            var blob = blobContainer.GetBlockBlobReference(blobName);
            await blob.UploadFromByteArrayAsync(emailAttachment, 0, emailAttachment.Length);

            return blob.Uri;
        }

        public async Task<Byte[]> DowloadFileFromBlobStorage(string filePath)
        {
            var memoryStream = new MemoryStream();
            var blob = new CloudBlockBlob(new Uri(filePath), new StorageCredentials(_azureStorageSettings.SasToken));
            await blob.DownloadToStreamAsync(memoryStream);
            
            return memoryStream.ToArray();
        }

        public async Task<string> DowloadFileFromBlobStorage(string blobName, string containerName)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(_azureStorageSettings.StorageConnectionString);
            // Create a blob client for interacting with the blob service
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            var blob = blobContainer.GetBlockBlobReference(blobName);
            var memoryStream = new MemoryStream();
            await blob.DownloadToStreamAsync(memoryStream);
            var fileContent =Encoding.UTF8.GetString(memoryStream.ToArray());

            return fileContent;
        }

        public async Task<string> DowloadTemplatejsonFromBlobStorage(string filePath)
        {
            var memoryStream = new MemoryStream();
            var blob = new CloudBlockBlob(new Uri(filePath), new StorageCredentials(_azureStorageSettings.SasToken));
            await blob.DownloadToStreamAsync(memoryStream);
             
            var json=Encoding.UTF8.GetString(memoryStream.ToArray());

            return json;
        }

        public async Task<Uri> UploadTwilioRecordingsToBlobStorage(string blobName, Byte[] recording)
        {
            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(_azureStorageSettings.StorageConnectionString);
            // Create a blob client for interacting with the blob service
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference("was-twilio-recordings");
            await blobContainer.CreateIfNotExistsAsync();

            var blob = blobContainer.GetBlockBlobReference(blobName);
            await blob.UploadFromByteArrayAsync(recording, 0, recording.Length);

            return blob.Uri;
        }

        /// <summary>
        /// Validate the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string</param>
        /// <returns>CloudStorageAccount object</returns>
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
