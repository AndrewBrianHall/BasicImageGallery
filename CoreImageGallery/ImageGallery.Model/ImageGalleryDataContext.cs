using ImageGallery.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageGallery.Model
{
    public class ImageGalleryDataContext : DbContext
    {
        public DbSet<UploadedImage> Images { get; set; }

        public ImageGalleryDataContext(DbContextOptions<ImageGalleryDataContext> options) : base(options)
        {

        }
    }

}
