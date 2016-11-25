using System;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Net;
using System.IO;

namespace EmotionalRatingBot.Storage
{
    public class BlobStorageProvider
    {
        public CloudBlockBlob SaveImage(string url, string type)
        {
            CloudStorageAccount storageAccount = ConfigurationProvider.CreateCloudStorageAccount();
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("photos");

            string imageName = $"{Guid.NewGuid().ToString()}.{type.Split('/')[1]}";
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);

            // Create the blob with contents from a file.
            var imageStream = this.GetStreamFromUrl(url);
            this.UpdateBlob(blockBlob, imageStream);

            return blockBlob;
        }

        public void UpdateBlob(CloudBlockBlob blob, Stream imageStream)
        {
            blob.UploadFromStream(imageStream);
        }

        private Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new WebClient())
            {
                imageData = wc.DownloadData(url);
            }

            return new MemoryStream(imageData);
        }
    }
}