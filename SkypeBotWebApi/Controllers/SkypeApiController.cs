using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SkypeBotWebApi.Controllers
{
    public class SkypeController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Test(string param1)
        {
            return Ok(param1);
        }
    }
}
