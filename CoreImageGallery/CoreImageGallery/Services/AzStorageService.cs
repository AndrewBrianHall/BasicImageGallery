using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoreImageGallery.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

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

        public AzStorageService(IConfiguration config)
        {
            _connectionString = config["AzureStorageConnectionString"];
            _account = CloudStorageAccount.Parse(_connectionString);
            _client = _account.CreateCloudBlobClient();
            _uploadContainer = _client.GetContainerReference("images");
            _publicContainer = _client.GetContainerReference("images-watermarked");
        }

        public async Task<Image> AddImageAsync(Stream stream, string fileName)
        {
            await _uploadContainer.CreateIfNotExistsAsync();

            fileName = ImagePrefix + fileName;
            var imageBlob = _uploadContainer.GetBlockBlobReference(fileName);
            await imageBlob.UploadFromStreamAsync(stream);

            return new Image()
            {
                FileName = fileName,
                ImagePath = imageBlob.Uri.ToString()
            };
        }

        public async Task InitializeBlobStorageAsync()
        {
            await _publicContainer.CreateIfNotExistsAsync();

            var permissions = await _publicContainer.GetPermissionsAsync();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off || permissions.PublicAccess == BlobContainerPublicAccessType.Unknown)
            {
                // If blob isn't public, we can't directly link to the pictures
                await _publicContainer.SetPermissionsAsync(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            await InitializeBlobStorageAsync();

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
