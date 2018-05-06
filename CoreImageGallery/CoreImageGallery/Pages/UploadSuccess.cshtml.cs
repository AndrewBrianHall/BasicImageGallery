using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreImageGallery.Pages
{
    public class UploadSuccessModel : PageModel
    {
        public string FileName { get; private set; }
        public string Time { get; private set; }

        public void OnGet(string file, string time)
        {
            this.FileName = Base64Encoder.DecodeBase64String(file);
            this.Time = Base64Encoder.DecodeBase64String(time);
        }
    }
}