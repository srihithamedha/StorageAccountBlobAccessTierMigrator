using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageAccountBlobAccessTierMigrator
{
    public static class BlobExtensions
    {
        public static async Task<List<CloudBlobContainer>> ListBlobContainers(this CloudBlobClient cloudBlobClient)
        {
            BlobContinuationToken continuationToken = null;
            var containers = new List<CloudBlobContainer>();

            do
            {
                try
                {
                    ContainerResultSegment response = await cloudBlobClient.ListContainersSegmentedAsync(continuationToken);
                    continuationToken = response.ContinuationToken;
                    containers.AddRange(response.Results);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (continuationToken != null);

            return containers;
        }

        public static async Task<List<CloudBlockBlob>> ListBlobs(this CloudBlobContainer cloudBlobContainer)
        {
            BlobContinuationToken continuationToken = null;
            var blobs = new List<CloudBlockBlob>();

            do
            {
                BlobResultSegment response = await cloudBlobContainer.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobs.AddRange(response.Results.Where(blobItem => blobItem.GetType() == typeof(CloudBlockBlob)).Cast<CloudBlockBlob>());
                blobs.AddRange(response.Results.Where(blobItem => blobItem.GetType() == typeof(CloudBlobDirectory)).SelectMany(directoryItem => ((CloudBlobDirectory)directoryItem).ListBlobsOfDirectory().GetAwaiter().GetResult()));
            } while (continuationToken != null);

            return blobs;
        }

        public async static Task<List<CloudBlockBlob>> ListBlobsOfDirectory(this CloudBlobDirectory cloudBlobDirectory)
        {
            BlobContinuationToken continuationToken = null;
            var blobs = new List<CloudBlockBlob>();

            do
            {
                try
                {
                    BlobResultSegment response = await cloudBlobDirectory.ListBlobsSegmentedAsync(continuationToken);
                    continuationToken = response.ContinuationToken;
                    blobs.AddRange(response.Results.Where(blobItem => blobItem.GetType() == typeof(CloudBlockBlob)).Cast<CloudBlockBlob>());
                    blobs.AddRange(response.Results.Where(blobItem => blobItem.GetType() == typeof(CloudBlobDirectory)).SelectMany(directoryItem => ((CloudBlobDirectory)directoryItem).ListBlobsOfDirectory().GetAwaiter().GetResult()));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            } while (continuationToken != null);

            return blobs;
        }
    }
}
