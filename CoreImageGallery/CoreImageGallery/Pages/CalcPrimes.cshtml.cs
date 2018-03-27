using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreImageGallery.Primes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoreImageGallery.Pages
{
    public class CalcPrimesModel : PageModel
    {
        ConcurrentBag<MinMaxPair> _primes = new ConcurrentBag<MinMaxPair>();

        [BindProperty]
        public int NumberOfSeconds { get; set; }
        public PrimeSummary Summary { get; set; } = null;

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            const int min = 0;
            const int max = 100000;

            Stopwatch stopwatch = Stopwatch.StartNew();
            int targetMilliseconds = this.NumberOfSeconds * 1000;

            while (stopwatch.ElapsedMilliseconds < targetMilliseconds)
            {
                var t1 = Task.Run(() => CalcPrimes(min, max));
                var t2 = Task.Run(() => CalcPrimes(min, max));
                await Task.WhenAll(new Task[] { t1, t2 });
            }

            stopwatch.Stop();

            this.Summary = new PrimeSummary
            {
                TotalCalcs = _primes.Count,
                ElapsedSeconds = stopwatch.ElapsedMilliseconds / 1000
            };
        }

        private void CalcPrimes(long min, long max)
        {
            var numbers = PrimeCalc.GetPrimes(min, max);
            _primes.Add(numbers);
        }
    }

    public class PrimeSummary
    {
        public int TotalCalcs { get; set; }
        public long ElapsedSeconds { get; set; }
    }
}