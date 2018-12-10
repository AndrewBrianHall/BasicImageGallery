using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageGallery.Model;

namespace CoreImageGallery.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<UploadedImage>> GetImagesAsync();
        Task AddImageAsync(Stream stream, string fileName, string userName);
    }
}