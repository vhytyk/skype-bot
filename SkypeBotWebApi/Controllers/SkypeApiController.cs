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
        private string GetSkypeNameByCruName(string cruName)
        {
            switch (cruName)
            {
                case "tkonyk":
                    return "tkonyk.ja";
                case "vhytyk":
                    return "victor.hytyk.prl";
                default:
                    return cruName;
            }
        }

        [HttpGet]
        public IHttpActionResult AddedOnReview(string reviewer, string cruid)
        {
            string skypeName = GetSkypeNameByCruName(reviewer);
            if (string.IsNullOrWhiteSpace(skypeName))
            {
                return BadRequest("can't find skype name for " + reviewer);
            }
            var rmqService = new RmqSkypeService();
            
            rmqService.PushMessage(new RmqSkypeMessage
            {
                Conversation = GetSkypeNameByCruName(reviewer),
                Message = string.Format("You were added on CR: http://crucible/cru/{0}", cruid)
            });
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult BuildEvent(string to, string job, string eventName, string revision=null, string node=null)
        {
            var rmqService = new RmqSkypeService();
            rmqService.PushMessage(new RmqSkypeMessage
            {
                Conversation = to,
                Message = string.Format("(*) {0} build of {1} branch *{2}* for {3} node", job, revision, eventName, node)
            });
            return Ok();
        }

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
