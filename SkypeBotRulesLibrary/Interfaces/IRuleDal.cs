using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBotRulesLibrary
{
    public interface IRuleDal
    {
        List<SkypeBotRule> GetAllRules();
        bool AddRule(SkypeBotRule newRule);
        SkypeBotRule GetById(int id);
        bool DeleteRule(int id);
    }
}
