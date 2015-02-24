using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace SkypeBotRulesLibrary
{
    public class BasicRuleService: IRuleService
    {
        private IRuleDal _dal;

        public BasicRuleService(IRuleDal dal)
        {
            _dal = dal;
        }

        public string GetApplicableRuleResult(string message)
        {
            List<SkypeBotRule> ruleList = GetAllRules();
           
            string result = null;
            foreach (SkypeBotRule rule in ruleList) 
            {
                Regex rx = new Regex(rule.Rule, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(message);
                if (matches.Count == 1)
                {
                    Match match = matches[0];
                    result = rule.Value;

                    GroupCollection groups = match.Groups;
                    
                    foreach (string groupName in rx.GetGroupNames())
                    {
                        result = result.Replace(string.Format("${0}$", groupName), groups[groupName].Value);
                    }
                    break;
                }
                
            }
            return result;
        }

        public bool AddRule(SkypeBotRule rule)
        {
            return _dal.AddRule(rule);
        }


        public List<SkypeBotRule> GetAllRules()
        {
            return _dal.GetAllRules();
        }

        public SkypeBotRule GetById(int id)
        {
            return _dal.GetById(id);
        }


        public bool DeleteRule(int id)
        {
            return _dal.DeleteRule(id);
        }
    }
}
