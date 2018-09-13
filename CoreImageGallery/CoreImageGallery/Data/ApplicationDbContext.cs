using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreImageGallery.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<UploadedImage> Images { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public async Task<UploadedImage> RecordImageUploadedAsync(string uploadId, string fileName, string imageUri, string userHash = null)
        {
            var img = new UploadedImage
            {
                Id = uploadId,
                FileName = fileName,
                ImagePath = imageUri,
                UploadTime = DateTime.Now,
                UserHash = userHash
            };

            await Images.AddAsync(img);
            await SaveChangesAsync();

            return img;
        }
    }
}
