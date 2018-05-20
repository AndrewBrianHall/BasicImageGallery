using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreImageGallery.Services;
using ImageGallery.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreImageGallery.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<UploadedImage> Images;

        private IStorageService _storageService;

        public IndexModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task OnGetAsync()
        {
            this.Images = await _storageService.GetImagesAsync();
        }

    }
}
