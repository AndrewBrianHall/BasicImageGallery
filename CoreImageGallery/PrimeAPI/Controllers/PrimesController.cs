using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrimeAPI.Primes;

namespace PrimeAPI.Controllers
{
    [Route("api/[controller]")]
    public class PrimesController : Controller
    {
        

        // GET: api/Primes
        [HttpGet]
        public string Get(long min, long max, int num)
        {


            return "";
        }

       
    }

}
