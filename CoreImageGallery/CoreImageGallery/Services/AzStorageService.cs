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
using ImageGallery.Model;
using CoreImageGallery.Data;

namespace CoreImageGallery.Services
{
    public class AzStorageService : IStorageService
    {
        private static bool ResourcesInitialized { get; set; } = false;

        private const string ImagePrefix = "img_";
        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient _client;
        private readonly string _connectionString;
        private CloudBlobContainer _uploadContainer;
        private CloudBlobContainer _publicContainer;

        private ApplicationDbContext _dbContext;

        public AzStorageService(IConfiguration config, ApplicationDbContext dbContext)
        {
            _connectionString = config["AzureStorageConnectionString"];
            _account = CloudStorageAccount.Parse(_connectionString);
            _client = _account.CreateCloudBlobClient();
            _uploadContainer = _client.GetContainerReference(Config.UploadContainer);
            _publicContainer = _client.GetContainerReference(Config.WatermarkedContainer);

            _dbContext = dbContext;
        }

        public async Task<UploadedImage> AddImageAsync(Stream stream, string originalName, string userName)
        {
            await InitializeResourcesAsync();

            string uploadId = Guid.NewGuid().ToString();
            string fileExtension = originalName.Substring(originalName.LastIndexOf('.'));
            string fileName = ImagePrefix + uploadId + fileExtension;
            string userHash = userName.GetHashCode().ToString();

            var imageBlob = _uploadContainer.GetBlockBlobReference(fileName);
            await imageBlob.UploadFromStreamAsync(stream);

            var img = new UploadedImage
            {
                Id = uploadId,
                FileName = fileName,
                ImagePath = imageBlob.Uri.ToString(),
                UploadTime = DateTime.Now,
                UserHash = userHash
            };

            await _dbContext.Images.AddAsync(img);
            await _dbContext.SaveChangesAsync();

            return img;
        }


        public async Task InitializeResourcesAsync()
        {
            if (!ResourcesInitialized)
            {
                //first Azure Storage resources
                await _publicContainer.CreateIfNotExistsAsync();

                await _uploadContainer.CreateIfNotExistsAsync();

                var permissions = await _publicContainer.GetPermissionsAsync();
                if (permissions.PublicAccess == BlobContainerPublicAccessType.Off || permissions.PublicAccess == BlobContainerPublicAccessType.Unknown)
                {
                    // If blob isn't public, we can't directly link to the pictures
                    await _publicContainer.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                ResourcesInitialized = true;
            }

        }

        public async Task<IEnumerable<UploadedImage>> GetImagesAsync()
        {
            await InitializeResourcesAsync();

            var imageList = new List<UploadedImage>();
            var token = new BlobContinuationToken();
            var blobList = await _publicContainer.ListBlobsSegmentedAsync(ImagePrefix, true, BlobListingDetails.All, 100, token, null, null);

            foreach (var blob in blobList.Results)
            {
                var image = new UploadedImage
                {
                    ImagePath = blob.Uri.ToString()
                };

                imageList.Add(image);
            }

            return imageList;
        }
    }
}
