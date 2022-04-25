using System;
using System.Collections.Generic;
using System.Text;

namespace WAS.Application.Common.Models
{
    public class FeedbackBlob : BlobRequest
    { 
        /// <summary>
        /// File base data
        /// </summary>
        public byte[] FileB64StringData { get; set; }
    }

    public class TemplateBlob : BlobRequest
    {
        /// <summary>
        /// File base data
        /// </summary>
        public string JsonData { get; set; }
    }


    public class BlobRequest
    {
        /// <summary>
        /// Blob container name
        /// </summary>
        public string BlobContainerName { get; set; }

        /// <summary>
        /// Blob type name
        /// </summary>
        public string BlobTypeName { get; set; }

        /// <summary>
        ///Blob file name
        /// </summary>
        public string BlobFileName { get; set; }

        /// <summary>
        /// Storage connection string
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Shared access expire time in minutes
        /// </summary>
        public int SharedAccessExpiryTimeInMinutes { get; set; }

    }

    public class BlobResponse
    {
        /// <summary>
        /// Blob uri
        /// </summary>
        public string BlobUri { get; set; }

        /// <summary>
        /// Blob name
        /// </summary>
        public string BlobName { get; set; }

    }
}
