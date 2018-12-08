using System;
using System.IO;
using ImageGallery.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Watermarker
{
    public static class ImageUploaded
    {
        const string WaterMarkText = "(c) CoreImageGallery";

        [FunctionName("ImageUploaded")]
        public static void Run([BlobTrigger("images/{name}")]Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               string name,
                               ILogger log)
        {
            try
            {
                WaterMarker.WriteWatermark(WaterMarkText, inputBlob, outputBlob);
                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            }
            catch (Exception e)
            {
                log.LogError($"Watermaking failed {e.Message}");
            }
        }
    }

}
