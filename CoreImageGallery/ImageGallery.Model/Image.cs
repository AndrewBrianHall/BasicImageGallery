using System;

namespace ImageGallery.Models
{
    public class Image
    {
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public DateTime UploadTime { get; set; }
        public string UploadUser { get; set; }
    }
}
