using SkypeBotRulesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SkypeBotWebApi.Controllers
{
    public class RulesController : ApiController
    {
        private readonly IRuleService _ruleService;

        public RulesController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        public IEnumerable<SkypeBotRule> Get()
        {
            return _ruleService.GetAllRules();
        }

        public SkypeBotRule Get(int id)
        {
            return _ruleService.GetById(id);
        }

        public bool Post([FromBody] SkypeBotRule newRule)
        {
            return _ruleService.AddRule(newRule);
        }
    }
}
