using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeBotRulesLibrary.Entities;

namespace SkypeBotRulesLibrary.Interfaces
{
    public interface ISkypeNameService
    {
        bool Add(SkypeNameInfo rule);
        List<SkypeNameInfo> GetAll();
        SkypeNameInfo GetById(int id);
        bool Delete(int id);

        string GetSkypeNameByDomainName(string cruName);
    }
}
