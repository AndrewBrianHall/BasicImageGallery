using CoreImageGallery.Data;
using ImageGallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreImageGallery.Services
{
    public class FileStorageService : IStorageService
    {
        private const string ImagePrefix = "img_";
        private const string ImageFolderUri = "userImages";
        private const string ImageFolder = "wwwroot\\" + ImageFolderUri;
        private ApplicationDbContext _dbContext;

        public FileStorageService(ApplicationDbContext dbContext)
        {
            _dbContext = null;
        }
        public async Task<UploadedImage> AddImageAsync(Stream stream, string originalName, string userName)
        {
            string uploadId = Guid.NewGuid().ToString();
            string fileExtension = Path.GetExtension(originalName);
            string fileName = ImagePrefix + uploadId + fileExtension;
            //string userHash = userName.GetHashCode().ToString();
            string localPath = Path.Combine(ImageFolder, fileName);
            string imageUri = ImageFolderUri + "/" + fileName;

            using(var fileStream = File.Create(localPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(fileStream);
            }

            var img = await _dbContext.RecordImageUploadedAsync(uploadId, fileName, imageUri);

            return img;
        }

        public async Task<IEnumerable<UploadedImage>> GetImagesAsync()
        {
            var imageList = new List<UploadedImage>();
            var files = Directory.EnumerateFiles(ImageFolder);

            foreach(var file in files)
            {
                var image = new UploadedImage
                {
                    ImagePath = ImageFolderUri + "/" + Path.GetFileName(file)
                };

                imageList.Add(image);
            }

            return imageList;
        }
    }
}
