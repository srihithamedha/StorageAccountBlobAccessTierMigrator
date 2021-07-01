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
        private readonly CloudStorageAccount storageAccount;

        public BlobAccessTierMigrator(string connectionString)
        {
            this.storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public BlobAccessTierMigrator(string accountName, string key)
        {
            this.storageAccount = CloudStorageAccount.Parse(
                string.Concat(
                    "DefaultEndpointsProtocol=https;AccountName=",
                    accountName,
                    ";AccountKey=",
                    key)
                );
        }

        public async Task ChangeAccessTierForAllContainers(StandardBlobTier blobTier)
        {
            var blobContainers = await this.storageAccount.CreateCloudBlobClient().ListBlobContainers();
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
