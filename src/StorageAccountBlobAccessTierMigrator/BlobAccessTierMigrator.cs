using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountBlobAccessTierMigrator
{
    public class BlobAccessTierMigrator
    {
        public CloudStorageAccount StorageAccount { get; set; }
        public BlobAccessTierMigrator(string connectionString)
        {
            this.StorageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public BlobAccessTierMigrator(string accountName, string key)
        {
            this.StorageAccount = CloudStorageAccount.Parse(
                string.Concat(
                    "DefaultEndpointsProtocol=https;AccountName=",
                    accountName,
                    ";AccountKey=",
                    key)
                );
        }

        public async Task ChangeAccessTier(StandardBlobTier blobTier)
        {
            var blobContainers = await this.StorageAccount.CreateCloudBlobClient().ListBlobContainers();
            blobContainers.ToList().ForEach(container =>
            {
                var blobs = container.ListBlobs().GetAwaiter().GetResult();
                blobs.ForEach(blob =>
                {
                    if (blob.Properties.StandardBlobTier != blobTier)
                    {
                        blob.SetStandardBlobTierAsync(blobTier).GetAwaiter().GetResult();
                    }
                });
            });
        }

    }
}
