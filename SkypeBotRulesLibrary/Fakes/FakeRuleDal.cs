using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace SkypeBotRulesLibrary.Fakes
{
    public class FakeRuleDal: IRuleDal
    {
        private const string CacheName = "skypeBotRulesCache";

        private List<SkypeBotRule> _defaultRules = new List<SkypeBotRule> { 
            new SkypeBotRule {
                Id = 1,
                Name = "Git History",
                Rule = @"\bcompare\s+(?<branch>[\w_-]+)\s+with\s+(?<masterBranch>[\w_-]+)\s+in\s+(?<repo>[\w_-]+)\b",
                Value = "http://highlander.justanswer.local:777/hli.aspx?repo=$repo$&branch=$branch$&masterBranch=$masterBranch$"
            },
            new SkypeBotRule {
                Id = 1,
                Name = "wiki ed",
                Rule = @"wiki ed",
                Value = "https://wiki/display/ua/Engineering+Days+Schedule"
            }
        };

        public List<SkypeBotRule> GetAllRules()
        {
            ObjectCache cache = MemoryCache.Default;
            var ruleList = cache[CacheName] as List<SkypeBotRule>;
            if (ruleList == null)
            {
                ruleList = _defaultRules;
                var policy = new CacheItemPolicy();
                cache.Set(CacheName, ruleList, policy);
            }
            return ruleList;
        }

        public bool AddRule(SkypeBotRule newRule)
        {
            bool result = false;
            if (newRule != null)
            {
                List<SkypeBotRule> currentList = GetAllRules();
                if (!currentList.Any(r => r.Name == newRule.Name)) 
                {
                    currentList.Add(newRule);
                    var policy = new CacheItemPolicy();
                    ObjectCache cache = MemoryCache.Default;
                    cache.Set(CacheName, currentList, policy);
                    result = true;
                }
            }
            return result;
        }
    }
}
