using System;
using Moq;
using NUnit.Framework;
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

            var handleMessageService = new HandleMessageService(service.Object, chatbot.Object);
            bool responded = false;
            handleMessageService.HandleIncomeMessage("testsource", message, (s, m) =>
            {
                Assert.IsNotEmpty(m);
                responded = true;
            });
            Assert.AreEqual(responded, shoudRespond);
        }
    }
}
