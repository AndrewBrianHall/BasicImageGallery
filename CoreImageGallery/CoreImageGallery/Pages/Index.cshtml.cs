using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreImageGallery.Services;
using ImageGallery.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Watermarker;

namespace CoreImageGallery.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger _logger;
        private const string UploadSuccessParameter = "uploadSuccess";
        public IEnumerable<UploadedImage> Images { get; set; }
        public bool UploadSuccess { get; set; } = false;

        private IStorageService _storageService;

        public IndexModel(IStorageService storageService, ILogger<IndexModel> logger)
        {
            _storageService = storageService;
            _logger = logger;
            Images = new List<UploadedImage>();
        }

        public async Task OnGetAsync(string value)
        {
            _logger.LogInformation("Getting images from storage - started.");
            try
            {
                this.Images = await _storageService.GetImagesAsync();
                _logger.LogInformation("Getting images from storage - complete.");
                _logger.LogInformation($"Getting images from storage - {this.Images.Count()}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Getting images from storage - error: {ex.ToString()}");
            }

            if (value == UploadSuccessParameter)
            {
                this.UploadSuccess = true;
            }
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            _logger.LogInformation($"Uploading {file.FileName} to storage - started.");
            string fileName = Path.GetFileName(file.FileName);
            DateTime now = DateTime.Now;
            string base64Time = Base64Encoder.GetBase64String($"{now.ToShortDateString()} {now.ToShortTimeString()}");
            string base64File = Base64Encoder.GetBase64String(fileName);
            ClaimsIdentity user = this.User.Identities.FirstOrDefault();

            _logger.LogInformation($"Watermarking {file.FileName} - started.");
            MemoryStream watermarkedImage = new MemoryStream();
            Stream originalImageStrm = file.OpenReadStream();
            WaterMarker.WriteWatermark(originalImageStrm, watermarkedImage, user.Name);
            _logger.LogInformation($"Watermarking {file.FileName} - completed.");

            await _storageService.AddImageAsync(watermarkedImage, fileName, user.Name);

            _logger.LogInformation($"Uploading {file.FileName} to storage - completed.");
            return RedirectToPage("UploadSuccess", new { file = base64File, time = base64Time });
        }

    }
}
