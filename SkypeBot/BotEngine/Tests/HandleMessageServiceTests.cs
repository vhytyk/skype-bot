using System;
using Moq;
using NUnit.Framework;
using SkypeBot.BotEngine.Commands;
using SkypeBot.SkypeDB;
using SkypeBotRulesLibrary;

namespace SkypeBot.BotEngine
{
    [TestFixture]
    public class HandleMessageServiceTests
    {
        [TestCase("bot, hello world", true)]
        [TestCase(" bot, hello world", true)]
        [TestCase(" bot,hello world", true)]
        [TestCase(". bot,hello world", false)]
        [TestCase("hello world bot", false)]
        [TestCase("bot", false)]
        [TestCase("beeee", false)]
        public void ChatBotResponseTest(string message, bool shoudRespond)
        {
            var service = new Mock<IRuleService>();
            service.Setup(s => s.GetApplicableRuleResult(It.IsAny<string>())).Returns(string.Empty);

            var chatbot = new Mock<IChatBotProvider>();
            chatbot.Setup(s => s.Think(It.IsAny<string>())).Returns(shoudRespond ? "hello!" : null);

            var skypeCommand = new Mock<ISkypeCommandProvider>();
            skypeCommand.Setup(s => s.GetCommand(It.IsAny<string>())).Returns((ISkypeCommand)null);

            var handleMessageService = new HandleMessageService(service.Object, chatbot.Object, skypeCommand.Object);
            bool responded = false;
            handleMessageService.HandleIncomeMessage("testsource", new SkypeMessage {Message = message}, (s, m) =>
            {
                Assert.IsNotEmpty(m);
                responded = true;
            });
            Assert.AreEqual(responded, shoudRespond);
        }

        [TestCase("bot#rl JA19189", "https://rally1.rallydev.com/#/detail/userstory/33811428317")]
        [TestCase("bot#rallylink DE11324", "https://rally1.rallydev.com/#/detail/defect/29658060588")]
        public void TestRallyCommand(string command, string response)
        {
            var service = new Mock<IRuleService>();
            service.Setup(s => s.GetApplicableRuleResult(It.IsAny<string>())).Returns(string.Empty);

            var chatbot = new Mock<IChatBotProvider>();
            chatbot.Setup(s => s.Think(It.IsAny<string>())).Returns((string)null);

            var handleMessageService = new HandleMessageService(service.Object, chatbot.Object, new SkypeCommandProvider());

            handleMessageService.HandleIncomeMessage("testsource", new SkypeMessage { Message = command }, (s, m) =>
            {
                Assert.IsNotEmpty(m);
                Assert.AreEqual(m, response);
            });
            

        }
    }
}
