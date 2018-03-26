using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreImageGallery.Models;

namespace CoreImageGallery.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<Image>> GetImagesAsync();
        Task<Image> AddImageAsync(Stream stream, string fileName);
    }
}