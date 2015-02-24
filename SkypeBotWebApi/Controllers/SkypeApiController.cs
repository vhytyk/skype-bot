using Newtonsoft.Json.Linq;
using SkypeBotRMQ;
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
        public IHttpActionResult SendMessage(string to, string msg)
        {
            var rmqService = new RmqSkypeService();
            rmqService.PushMessage(new RmqSkypeMessage { 
                Conversation = to,
                Message = msg
            });
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult SendMessagePost(RmqSkypeMessage jsonData)
        {
            var rmqService = new RmqSkypeService();
            rmqService.PushMessage(jsonData);
            return Ok();
        }
    }
}
