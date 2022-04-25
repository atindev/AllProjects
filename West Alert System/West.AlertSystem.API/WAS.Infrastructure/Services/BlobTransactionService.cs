using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;
using WAS.Application.Interface.Services;

namespace WAS.Infrastructure.Services
{
    public class BlobTransactionService : IBlobTransactionService
    {
        /// <summary>
        /// Upload  feedback screens
        /// </summary>
        /// <param name="newBlob"></param>
        /// <returns>blobResponse</returns>
        public BlobResponse UploadToBlob(FeedbackBlob newBlob)
        {
            BlobResponse blobResponse = SaveFileToBlob(newBlob).Result;

            return blobResponse;
        }

        /// <summary>
        /// Upload  blob storage screens 
        /// </summary>
        /// <param name="newBlob"></param>
        /// <returns>blobResponse</returns>
        public async Task<BlobResponse> UploadToBlobAsync(FeedbackBlob newBlob)
        {
            BlobResponse blobResponse = await SaveFileToBlob(newBlob);
            return blobResponse;
        }

        /// <summary>
        /// for saving template as JSON files
        /// </summary>
        /// <param name="newBlob"></param>
        /// <returns></returns>
        public async Task<BlobResponse> UploadJSONToBlobAsync(TemplateBlob newBlob)
        {
            BlobResponse blobResponse = await SaveJSONFileToBlob(newBlob);
            return blobResponse;
        }

        /// <summary>
        /// Save files
        /// </summary>
        /// <param name="newBlob"></param>
        /// <returns>blobResponse</returns>
        private async Task<BlobResponse> SaveFileToBlob(FeedbackBlob newBlob)
        {
            BlobResponse blobResponse = new BlobResponse();
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(newBlob.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(newBlob.BlobContainerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (newBlob.BlobFileName != null && newBlob.FileB64StringData != null)
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(newBlob.BlobFileName);
                cloudBlockBlob.Properties.ContentType = newBlob.BlobTypeName;
                await cloudBlockBlob.UploadFromByteArrayAsync(newBlob.FileB64StringData, 0, newBlob.FileB64StringData.Length);
                blobResponse.BlobUri = cloudBlockBlob.Uri.AbsoluteUri;
                blobResponse.BlobName = cloudBlockBlob.Name;
            }
            return blobResponse;
        }

        /// <summary>
        /// Save files
        /// </summary>
        /// <param name="newBlob"></param>
        /// <returns>blobResponse</returns>
        private async Task<BlobResponse> SaveJSONFileToBlob(TemplateBlob newBlob)
        {
            BlobResponse blobResponse = new BlobResponse();
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(newBlob.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(newBlob.BlobContainerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (newBlob.BlobFileName != null && newBlob.JsonData != null)
            {
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(newBlob.BlobFileName);
                cloudBlockBlob.Properties.ContentType = newBlob.BlobTypeName;

                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(newBlob.JsonData)))
                {
                    await cloudBlockBlob.UploadFromStreamAsync(ms);
                }
                 
                blobResponse.BlobUri = cloudBlockBlob.Uri.AbsoluteUri;
                blobResponse.BlobName = cloudBlockBlob.Name;
            }
            return blobResponse;
        }

        /// <summary>
        /// Get file name from blobrequest
        /// </summary>
        /// <param name="blobRequest"></param>
        /// <returns>fileUrl</returns>
        public string GetFileUrl(BlobRequest blobRequest)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobRequest.StorageConnectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(blobRequest.BlobContainerName);

            SharedAccessBlobPolicy sharedAccessBlobPolicy = new SharedAccessBlobPolicy();
            sharedAccessBlobPolicy.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15);
            sharedAccessBlobPolicy.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(blobRequest.SharedAccessExpiryTimeInMinutes);
            sharedAccessBlobPolicy.Permissions = SharedAccessBlobPermissions.Read;

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobRequest.BlobFileName);
            string sas = cloudBlockBlob.GetSharedAccessSignature(sharedAccessBlobPolicy);
            string fileUrl = cloudBlockBlob.Uri.AbsoluteUri + sas;
            return fileUrl;
        }

    }
}
