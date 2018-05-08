using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageGallery.Model;
using ImageGallery.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Watermarker
{
    public static class GalleryImageUploaded
    {

        [FunctionName("Watermarker")]
        public static void Run([BlobTrigger("images/{name}")]Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               [CosmosDB(Config.DatabaseId, Config.CollectionId, ConnectionStringSetting = "CosmosConnection")] IEnumerable<UploadedImage> images,
                               string name,
                               TraceWriter log)
        {
            try
            {
                UploadedImage current = images.Where(i => i.FileName == name).ToList().FirstOrDefault();
                string uploadUser = current.UploadUser;
                if (uploadUser.Length > 10)
                {
                    uploadUser.Substring(0, 10);
                }

                string waterMark = $"(c) {uploadUser}";
                ImageMarker.WriteWatermark(waterMark, inputBlob, outputBlob);
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            }
            catch (Exception e)
            {
                log.Error($"Watermarking failed: {e.Message}");
            }
        }
    }

}
