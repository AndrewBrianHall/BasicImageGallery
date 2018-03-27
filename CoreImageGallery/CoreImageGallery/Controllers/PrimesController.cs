using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoreImageGallery.Primes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreImageGallery.Controllers
{
    [Produces("application/json")]
    [Route("api/Primes")]
    public class PrimesController : Controller
    {
        ConcurrentBag<MinMaxPair> _primes = new ConcurrentBag<MinMaxPair>();

        // GET: api/Primes
        [HttpGet]
        public async Task<PrimeSummary> Get(long min, long max, int num)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int targetMilliseconds = num * 1000;

            while (stopwatch.ElapsedMilliseconds < targetMilliseconds)
            {
                var t1 = Task.Run(() => CalcPrimes(min, max, num));
                var t2 = Task.Run(() => CalcPrimes(min, max, num));
                await Task.WhenAll(new Task[] { t1, t2});
            }

            stopwatch.Stop();

            var summary = new PrimeSummary
            {
                TotalCalcs = _primes.Count,
                ElapsedSeconds = stopwatch.ElapsedMilliseconds / 1000
            };

            return summary;
        }

        private void CalcPrimes(long min, long max, int num)
        {
            for (int i = 0; i < num; i++)
            {
                var numbers = PrimeCalc.GetPrimes(min, max);
                _primes.Add(numbers);
            }
        }
    }

    public class PrimeSummary
    {
        public int TotalCalcs { get; set; }
        public long ElapsedSeconds { get; set; }
    }
}
