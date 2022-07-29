using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace vbSparkle.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeobfuscateController : ControllerBase
    {
        // GET: api/Deob
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "OK" };
        }

        // POST: api/Deob
        [HttpPost]
        [Consumes("text/plain")]
        [Produces("text/plain")]
        public string Post([FromBody] string obfuscatedCode)
        {
            return VbPartialEvaluator.PrettifyEncoded(obfuscatedCode);
        }
    }
}
