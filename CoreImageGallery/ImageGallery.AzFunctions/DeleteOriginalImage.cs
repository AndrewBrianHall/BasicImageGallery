using System;
using System.IO;
using System.Threading.Tasks;
using ImageGallery.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ImageGallery.AzFunctions
{

    public static class DeleteOriginalImage
    {

        [FunctionName("DeleteOriginalImage")]
        public static async Task Run([BlobTrigger(Config.WatermarkedContainer + "/{name}", 
            Connection = "AzureWebJobsStorage")]Stream myBlob, 
            string name, 
            ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer uploadContainer = client.GetContainerReference(Config.UploadContainer);

            CloudBlobContainer container = client.GetContainerReference(Config.UploadContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);

            await blockBlob.DeleteIfExistsAsync();

            log.LogInformation("Deleted original: {0}", name);
        }
    }
}
