using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using ImageGallery.Models;

namespace CoreImageGallery.Services
{
    public class AzStorageService : IStorageService
    {
        private const string ImagePrefix = "img_";

        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _client;
        private readonly string _connectionString;
        private CloudBlobContainer _uploadContainer;
        private CloudBlobContainer _publicContainer;

        private string _cosmosEndpointUrl;
        private string _cosmosPrimaryKey; 
        private DocumentClient _cosmosClient;

        public AzStorageService(IConfiguration config)
        {
            _connectionString = config["AzureStorageConnectionString"];
            _account = CloudStorageAccount.Parse(_connectionString);
            _client = _account.CreateCloudBlobClient();
            _uploadContainer = _client.GetContainerReference("images");
            _publicContainer = _client.GetContainerReference("images-watermarked");
            _cosmosEndpointUrl = config["CosmosConnectionString"];
            _cosmosPrimaryKey = config["CosmosPrimaryKey"];
            _cosmosClient = new DocumentClient(new Uri(_cosmosEndpointUrl), _cosmosPrimaryKey);
        }

        public async Task<Image> AddImageAsync(Stream stream, string fileName, string userName)
        {
            await _uploadContainer.CreateIfNotExistsAsync();

            fileName = ImagePrefix + fileName;
            var imageBlob = _uploadContainer.GetBlockBlobReference(fileName);
            await imageBlob.UploadFromStreamAsync(stream);
            var img = new Image
            {
                FileName = fileName,
                ImagePath = imageBlob.Uri.ToString(),
                UploadTime = DateTime.Now,
                UploadUser = userName
            };

            await RecordUploadAsync(img);

            return img;

        }

        private async Task RecordUploadAsync(Image img)
        {
            //persist this image with upload audit details to db
            await this._cosmosClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("images", "coll"), img);
 

        }

     
        public async Task InitializeResourcesAsync()
        {
            //first Azure Storage resources
            await _publicContainer.CreateIfNotExistsAsync();

            var permissions = await _publicContainer.GetPermissionsAsync();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off || permissions.PublicAccess == BlobContainerPublicAccessType.Unknown)
            {
                // If blob isn't public, we can't directly link to the pictures
                await _publicContainer.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            //next Azure CosmosDb resources
            await _cosmosClient.CreateDatabaseIfNotExistsAsync(new Database { Id = "images" });

            DocumentCollection myCollection = new DocumentCollection();
            myCollection.Id = "coll";
            myCollection.PartitionKey.Paths.Add("/deviceId");

            await _cosmosClient.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri("images"),
                myCollection,
                new RequestOptions { OfferThroughput = 2500 });
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            await InitializeResourcesAsync();

            var imageList = new List<Image>();
            var token = new BlobContinuationToken();
            var blobList = await _publicContainer.ListBlobsSegmentedAsync(ImagePrefix, true, BlobListingDetails.All, 100, token, null, null);
            
            foreach (var blob in blobList.Results)
            {
                var image = new Image
                {
                    ImagePath = blob.Uri.ToString()
                };

                imageList.Add(image);
            }

            return imageList;
        }
    }
}
