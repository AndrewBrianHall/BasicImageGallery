using System;
using System.Collections.Generic;
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
        // GET: api/Primes
        [HttpGet]
        public string Get(long min, long max)
        {
            var primes = PrimeCalc.GetPrimesAsJson(min, max);
            return primes;
        }


    }
}
