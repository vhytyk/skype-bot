using System;
using System.Diagnostics;
using ChatterBotAPI;
using SkypeBotRulesLibrary;

namespace SkypeBot.BotEngine
{
    public class HandleMessageService : IHandleMessageService
    {
        private readonly ChatterBotSession _chatterBot;
        private readonly IRuleService _ruleService;

        public HandleMessageService(IRuleService ruleService)
        {
            _chatterBot = new ChatterBotFactory().Create(ChatterBotType.PANDORABOTS, "b0dafd24ee35a477").CreateSession();
            _ruleService = ruleService;
        }
        public void HandleIncomeMessage(string source, string message, Action<string, string> responseAction)
        {
            Debug.WriteLine("Message received from {0}: {1}", source, message);
            string ruleServiceResponse = _ruleService.GetApplicableRuleResult(message);
            if(!string.IsNullOrEmpty(ruleServiceResponse))
            {
                responseAction(source, ruleServiceResponse);
            }
            else
            {
                string response = _chatterBot.Think(message);
                responseAction(source, response);
            }
        }
    }
}
