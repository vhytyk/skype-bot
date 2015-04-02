using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using SkypeBotRulesLibrary.Entities;
using SkypeBotRulesLibrary.Interfaces;

namespace SkypeBotRulesLibrary.Implementations
{
    public class SkypeNameService : ISkypeNameService
    {
        private ISkypeNameDal _skypeNameDal = new SkypeNameDal();

        public bool Add(Entities.SkypeNameInfo skypeNameInfo)
        {
            return _skypeNameDal.Add(skypeNameInfo);
        }

        public List<Entities.SkypeNameInfo> GetAll()
        {
            return _skypeNameDal.GetAll();
        }

        public Entities.SkypeNameInfo GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            return _skypeNameDal.Delete(id);
        }

        public string GetSkypeNameByDomainName(string domainName)
        {
            SkypeNameInfo val = _skypeNameDal.GetByDomainName(domainName);
            return val != null ? val.SkypeName : null;
        }
    }
}
