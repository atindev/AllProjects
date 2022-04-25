using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WAS.Application.Common.Models;

namespace WAS.Application.Interface.Services
{
    public interface IBlobTransactionService
    {
        BlobResponse UploadToBlob(FeedbackBlob newBlob);
        string GetFileUrl(BlobRequest blobRequest);
        Task<BlobResponse> UploadToBlobAsync(FeedbackBlob newBlob);
        Task<BlobResponse> UploadJSONToBlobAsync(TemplateBlob newBlob);

    }
}
