using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Primes.Lib;

namespace CoreImageGallery.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }
        public long Prime { get; set; }

        public void OnGet()
        {
            Prime = PrimeCalc.GetPrimes(0, 25000).Max;
            Message = "Your contact page.";
        }
    }
}
