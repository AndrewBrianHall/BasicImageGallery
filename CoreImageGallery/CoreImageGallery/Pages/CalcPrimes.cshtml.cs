using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Primes.Lib;

namespace CoreImageGallery.Pages
{
    public class CalcPrimesModel : PageModel
    {
        [BindProperty]
        public int NumberOfSeconds { get; set; }
        public PrimeSummary Summary { get; set; } = null;

        string _apiURL = null;

        public CalcPrimesModel(IConfiguration config)
        {
            _apiURL = config["PrimeAPIUrl"];
        }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            const int min = 0;
            const int max = 100000;

            if (_apiURL == null)
            {
                var primeCalc = new PrimeCalc();
                this.Summary = await primeCalc.CalcPrimesForTime(min, max, this.NumberOfSeconds);
            }
            else
            {
                this.Summary = await GetPrimesAsync(_apiURL, min, max, this.NumberOfSeconds);
            }
        }

        private static async Task<PrimeSummary> GetPrimesAsync(string baseUrl, long min, long max, long time)
        {
            var url = baseUrl + $"/api/primes?min={min}&max={max}&num={time}";
            var req = WebRequest.Create(url);

            var resp = await req.GetResponseAsync() as HttpWebResponse;
            var str = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(resp.CharacterSet));
            var json = await str.ReadToEndAsync();
            var summary = JsonConvert.DeserializeObject<PrimeSummary>(json);
            return summary;
        }

    }

}