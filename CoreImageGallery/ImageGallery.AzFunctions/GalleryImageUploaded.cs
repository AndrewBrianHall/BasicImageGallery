using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Watermarker
{
    public static class GalleryImageUploaded
    {
        const string WatermarkMessage = "CoreImageGallery";
        
        [FunctionName("Watermarker")]
        public static void Run([BlobTrigger("images/{name}")]Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               string name, 
                               TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");

            try
            {
                ImageMarker.WriteWatermark(WatermarkMessage, inputBlob, outputBlob);
            }
            catch (Exception e)
            {
                log.Info($"Watermarking failed: {e.Message}");
            }
        }
    }
}
