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
    public static class ImageUploaded
    {

        [FunctionName("ImageUploaded")]
        public static void Run([BlobTrigger(Config.UploadContainer + "/{name}")]Stream inputBlob,
                               [Blob(Config.WatermarkedContainer + "/{name}", FileAccess.Write)] Stream outputBlob,
                               [CosmosDB(Config.DatabaseId, Config.CollectionId, ConnectionStringSetting = "CosmosConnection")] IEnumerable<UploadedImage> images,
                               string name,
                               TraceWriter log)
        {
            try
            {
                UploadedImage current = images.Where(i => i.FileName == name).ToList().FirstOrDefault();
                string uploadUser = current.UploadUser;
                if (uploadUser.Length > 32)
                {
                    uploadUser = uploadUser.Substring(0, 10);
                }

                string waterMark = $"(c) {uploadUser}";
                ImageMarker.WriteWatermark(waterMark, inputBlob, outputBlob);
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            }
            catch(Exception e)
            {
                log.Error($"Watermaking failed {e.Message}");
            }
        }
    }

}
