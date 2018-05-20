using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageGallery.Model;
using ImageGallery.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Watermarker
{
    public static class ImageUploaded
    {
        const string WaterMarkText = "(c) CoreImageGallery";

        [FunctionName("ImageUploaded")]
        public static void Run([BlobTrigger(Config.UploadContainer + "/{name}")]Stream inputBlob,
                               [Blob(Config.WatermarkedContainer + "/{name}", FileAccess.Write)] Stream outputBlob,
                               string name,
                               TraceWriter log)
        {
            try
            {
                WaterMarker.WriteWatermark(WaterMarkText, inputBlob, outputBlob);
                log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");
            }
            catch(Exception e)
            {
                log.Error($"Watermaking failed {e.Message}");
            }
        }
    }

}
