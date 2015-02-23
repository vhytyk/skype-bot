using NUnit.Framework;
using SkypeBotRulesLibrary.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBotRulesLibrary.Tests
{
    [TestFixture]
    class BasicRuleServiceTests
    {
        [Test]
        public void TestGitHistoryRule()
        {
            FakeRuleDal dal = new FakeRuleDal();
            BasicRuleService service = new BasicRuleService(dal);

            const string message = "compare HighlanderIntegration with SystemIntegration in ja-dotnet";
            string result = service.GetApplicableRuleResult(message);
            Assert.AreEqual("http://highlander.justanswer.local:777/hli.aspx?repo=ja-dotnet&branch=HighlanderIntegration&masterBranch=SystemIntegration", result);
        }
    }
}
