using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreImageGallery.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreImageGallery.Pages
{
    public class UploadModel : PageModel
    {
        private IStorageService _storageService;

        public UploadModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public void OnGet()
        {

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