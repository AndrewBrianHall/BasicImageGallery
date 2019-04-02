using System;

namespace ImageGallery.Model
{
    public class UploadedImage
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public DateTime UploadTime { get; set; }
        public string UserHash { get; set; }
    }
}
