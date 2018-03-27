using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Primes.Lib;

namespace PrimeAPI.Controllers
{
    [Route("api/[controller]")]
    public class PrimesController : Controller
    {
        // GET: api/Primes
        [HttpGet]
        public async Task<PrimeSummary> Get(long min, long max, long num)
        {
            var primeCalc = new PrimeCalc();
            var summary = await primeCalc.CalcPrimesForTime(min, max, num);

            return summary;
        }

    }

}
