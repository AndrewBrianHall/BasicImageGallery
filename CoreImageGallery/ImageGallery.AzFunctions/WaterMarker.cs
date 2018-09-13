using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Watermarker
{
    public class WaterMarker
    {
        public static void WriteWatermark(string watermarkContent, Stream originalImageStrm, Stream newImageStrm)
        {
            originalImageStrm.Position = 0;

            using (Image inputImage = Image.FromStream(originalImageStrm, true))
            using (Graphics graphic = Graphics.FromImage(inputImage))
            {
                Font font = new Font("Georgia", 12, FontStyle.Bold);
                SizeF textSize = graphic.MeasureString(watermarkContent, font);

                float xPostFromLeft = textSize.Width/2 + 10;
                float yPosFromBottom = (int)(inputImage.Height) - (textSize.Height) - 10;

                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                StringFormat StrFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center
                };

                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));
                graphic.DrawString(watermarkContent, font, semiTransBrush2, xPostFromLeft + 1, yPosFromBottom + 1, StrFormat);

                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));
                graphic.DrawString(watermarkContent, font, semiTransBrush, xPostFromLeft, yPosFromBottom, StrFormat);
                graphic.Flush();

                //Saving the image contents directly into the output stream doesn't work in V2 functions
                //workaround is to write to a memory stream first and copy to the output stream
                Stream tempStrm = new MemoryStream();
                inputImage.Save(tempStrm, inputImage.RawFormat);
                tempStrm.Seek(0, SeekOrigin.Begin);
                tempStrm.CopyTo(newImageStrm);
            }
        }
    }
}
