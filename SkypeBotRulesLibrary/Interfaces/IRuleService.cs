using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBotRulesLibrary
{
    public interface IRuleService
    {
        string GetApplicableRuleResult(string message);
        bool AddRule(SkypeBotRule rule);
        List<SkypeBotRule> GetAllRules();
        SkypeBotRule GetById(int id);
        bool DeleteRule(int id);
    }
}
