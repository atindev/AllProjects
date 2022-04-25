using System;
using System.Threading.Tasks;

namespace WAS.Application.Interface.Services
{
    public interface IAzureStorageService
    {
        Task AddMessageToStorageQueue(object messageToQueue, string queueName);

        Task<Uri> UploadFileToBlobStorage(string blobName, Byte[] emailAttachment);

        Task<Byte[]> DowloadFileFromBlobStorage(string filePath);

        Task<string> DowloadFileFromBlobStorage(string blobName, string containerName);

        Task<string> DowloadTemplatejsonFromBlobStorage(string filePath);

        Task<Uri> UploadTwilioRecordingsToBlobStorage(string blobName, Byte[] recording);
    }
}
