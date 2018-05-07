using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreImageGallery.Models;
using CoreImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreImageGallery.Pages
{
    public class IndexModel : PageModel
    {
        private const string UploadSuccessParameter = "uploadSuccess";
        public IEnumerable<Image> Images;
        public bool UploadSuccess = false;

        private IStorageService _storageService;

        public IndexModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task OnGetAsync(string value)
        {
            this.Images = await _storageService.GetImagesAsync();

            if (value == UploadSuccessParameter)
            {
                this.UploadSuccess = true;
            }
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            DateTime now = DateTime.Now;
            var base64Time = Base64Encoder.GetBase64String($"{now.ToShortDateString()} {now.ToShortTimeString()}");
            var base64File = Base64Encoder.GetBase64String(fileName);
            var user = User.Identities.FirstOrDefault();

            await _storageService.AddImageAsync(file.OpenReadStream(), fileName, user.Name);

            return RedirectToPage("UploadSuccess", new { file = base64File, time = base64Time });
        }

    }
}
