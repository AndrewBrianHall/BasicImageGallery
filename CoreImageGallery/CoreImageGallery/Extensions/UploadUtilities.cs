﻿using CoreImageGallery.Data;
using ImageGallery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreImageGallery.Extensions
{
    public class UploadUtilities
    {
        private const string ImagePrefix = "img_";

        public static void GetImageProperties(string originalName, out string uploadId, out string fileName)
        {
            uploadId = Guid.NewGuid().ToString();
            string fileExtension = Path.GetExtension(originalName);
            fileName = ImagePrefix + uploadId + fileExtension;
        }

        public static async Task RecordImageUploadedAsync(ApplicationDbContext dbContext, string uploadId, string fileName, string imageUri, string userHash = null)
        {
            var img = new UploadedImage
            {
                Id = uploadId,
                FileName = fileName,
                ImagePath = imageUri,
                UploadTime = DateTime.Now,
                UserHash = userHash
            };

            await dbContext.Images.AddAsync(img);
            await dbContext.SaveChangesAsync();
        }
    }
}
