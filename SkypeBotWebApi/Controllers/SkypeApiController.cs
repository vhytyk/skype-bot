using Newtonsoft.Json.Linq;
using SkypeBotRMQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using SkypeBotRulesLibrary.Entities;
using SkypeBotRulesLibrary.Implementations;
using SkypeBotRulesLibrary.Interfaces;
using SkypeBotWebApi.Models;

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
        public IHttpActionResult ReviewCommented(string reviewer, string cruid, string by)
        {
            return SendCodeReviewMessage(reviewer, cruid, string.Format("Your review has been commented by {0}", by));
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

        [HttpPost]
        public IHttpActionResult Jenkins(JenkinsNotification jData)
        {
            var rmqService = new RmqSkypeService();
            UniversalDal<JenkinsSubscription> dal = new UniversalDal<JenkinsSubscription>();
            dal.GetAll().Where(item => item.JenkisJobName == jData.Name && item.Active).ToList().ForEach(subscription =>
            {
                rmqService.PushMessage(new RmqSkypeMessage()
                {
                    Conversation = subscription.ConversationName,
                    Message =
                        string.Format("(*) Build for {0} - {1}{2}",
                            jData.Name,
                            jData.Build.Phase,
                            string.IsNullOrEmpty(jData.Build.Status) ? "" : ", status: *" + jData.Build.Status + "*")
                });
            });
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Ping(object jsonData)
        {
            return new System.Web.Http.Results.FormattedContentResult<string>(HttpStatusCode.OK,
                "pong",
                new JsonMediaTypeFormatter(),
                new MediaTypeHeaderValue("application/json"),
                this);
        }

    }
}
