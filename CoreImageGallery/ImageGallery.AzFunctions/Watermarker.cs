using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace Watermarker
{
    public static class Watermarker
    {
        private static void WriteWatermark(string watermarkContent, Stream originalImage, Stream newImage)
        {
            originalImage.Position = 0;

            using (Image inputImage = Image.FromStream(originalImage, true))
            using (Graphics graphic = Graphics.FromImage(inputImage))
            {
                Font font = new Font("Georgia", 12, FontStyle.Bold);
                SizeF textSize = graphic.MeasureString(watermarkContent, font);

                float xCenterOfImg = (inputImage.Width / 2);
                float yPosFromBottom = (int)(inputImage.Height * 0.90) - (textSize.Height / 2);

                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                StringFormat StrFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };

                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
                graphic.DrawString(watermarkContent, font, semiTransBrush2, xCenterOfImg + 1, yPosFromBottom + 1, StrFormat);

                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
                graphic.DrawString(watermarkContent, font, semiTransBrush, xCenterOfImg, yPosFromBottom, StrFormat);

                graphic.Flush();
                inputImage.Save(newImage, ImageFormat.Jpeg);
            }
        }

        [FunctionName("Watermarker")]
        public static void Run([BlobTrigger("images/{name}")]Stream inputBlob,
                               [Blob("images-watermarked/{name}", FileAccess.Write)] Stream outputBlob,
                               string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputBlob.Length} Bytes");

            try
            {
                var message = "CoreImageGallery";
                WriteWatermark(message, inputBlob, outputBlob);
            }
            catch (Exception e)
            {
                log.Info($"Watermarking failed: {e.Message}");
            }
        }
    }
}
