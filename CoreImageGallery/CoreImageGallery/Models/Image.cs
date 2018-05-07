using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImageGallery.Models
{
    public class Image
    {
        public string FileName { get; set; }
        public string ImagePath { get; set; }
        public DateTime UploadTime { get; set; }
        public string UploadUser { get; set; }
    }
}