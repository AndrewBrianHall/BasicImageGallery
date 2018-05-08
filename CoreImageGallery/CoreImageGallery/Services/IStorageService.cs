using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageGallery.Models;

namespace CoreImageGallery.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<UploadedImage>> GetImagesAsync();
        Task<UploadedImage> AddImageAsync(Stream stream, string userName);
    }
}