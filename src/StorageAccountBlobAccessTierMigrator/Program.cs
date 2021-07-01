using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace StorageAccountBlobAccessTierMigrator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string connectionString = "<connection-string>";
            await new BlobAccessTierMigrator(connectionString).ChangeAccessTierForAllContainers(StandardBlobTier.Archive);

            // For Connection with account name and key
            //string accountName = "<account-name>", key = "<account-key>";
            //await new BlobAccessTierMigrator(accountName, key).ChangeAccessTierForAllContainers(StandardBlobTier.Archive);
        }
    }
}
