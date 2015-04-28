using Newtonsoft.Json.Linq;
using SkypeBotRMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkypeBotRulesLibrary.Implementations;
using SkypeBotRulesLibrary.Interfaces;

namespace SkypeBotWebApi.Controllers
{
    public class SkypeController : ApiController
    {
        private ISkypeNameService _skypeNameService;

        public SkypeController()
        {
            _skypeNameService = new SkypeNameService();
        }
        private string GetSkypeNameByCruName(string cruName)
        {
            string skypeName = _skypeNameService.GetSkypeNameByDomainName(cruName) ?? cruName;
            return skypeName;
        }

        private IHttpActionResult SendCodeReviewMessage(string reviewer, string cruid, string message)
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
                Message = string.Format("{0}: http://crucible/cru/{1}", message, cruid)
            });

             return Ok();
        }

        [HttpGet]
        public IHttpActionResult UpdatedReview(string reviewer, string cruid)
        {
            return SendCodeReviewMessage(reviewer, cruid, "CR updated");
        }
        [HttpGet]
        public IHttpActionResult AddedOnReview(string reviewer, string cruid)
        {
            return SendCodeReviewMessage(reviewer, cruid, "You were added on CR");
        }

        [HttpGet]
        public IHttpActionResult BranchLock(string locker, string branch, string command)
        {
            string skypeName = GetSkypeNameByCruName(locker) ?? locker;
            var rmqService = new RmqSkypeService();

            rmqService.PushMessage(new RmqSkypeMessage
            {
                Conversation = "Merge masters test chat",
                Message = string.Format("[{0}] {1} {2}", skypeName, command, branch)
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
