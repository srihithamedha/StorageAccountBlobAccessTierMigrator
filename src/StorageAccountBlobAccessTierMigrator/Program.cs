using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace StorageAccountBlobAccessTierMigrator
{
    class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "<connection-string>";
            new BlobAccessTierMigrator(connectionString).ChangeAccessTier(StandardBlobTier.Archive).Wait();
            // For Connection with account name and key
            //new BlobAccessTierMigrator(accountName, key).ChangeAccessTier(StandardBlobTier.Archive).Wait();
        }
    }
}
