using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeBotRulesLibrary.Entities;

namespace SkypeBotRulesLibrary.Interfaces
{
    public interface ISkypeNameDal
    {
        List<SkypeNameInfo> GetAll();
        bool Add(SkypeNameInfo skypeNameInfo);
        SkypeNameInfo GetById(int id);
        bool Delete(int id);

        SkypeNameInfo GetByDomainName(string domainName);
    }
}
