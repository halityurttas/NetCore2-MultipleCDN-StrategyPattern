using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace CoreCDNSample.Components.CDN.Concrete
{
    public class AzureCDN : ICDN
    {
        string connectionString;
        string container;

        public AzureCDN(string connectionString, string container)
        {
            this.connectionString = connectionString;
            this.container = container;
        }

        public string GetPath(string relativePath)
        {
            try
            {
                var container = getCloudBlobContainer();
                var blob = container.GetBlockBlobReference(relativePath);
                if (blob != null)
                {
                    return blob.Uri.OriginalString;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Save(byte[] file, string name, string folder)
        {
            try
            {
                var container = getCloudBlobContainer();
                var block = container.GetBlockBlobReference(folder + "/" + name);
                block.UploadFromByteArrayAsync(file, 0, file.Length).Wait();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Utils

        private CloudBlobContainer getCloudBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference(container);
            blobContainer.CreateIfNotExistsAsync().Wait();
            blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob }).Wait();
            return blobContainer;
        }

        #endregion
    }
}
