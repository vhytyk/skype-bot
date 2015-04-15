using System;
using System.Diagnostics;
using ChatterBotAPI;
using SkypeBotRulesLibrary;
using System.Text.RegularExpressions;

namespace SkypeBot.BotEngine
{
    public class HandleMessageService : IHandleMessageService
    {
        private readonly IChatBotProvider _chatterBot;
        private readonly IRuleService _ruleService;

        public HandleMessageService(IRuleService ruleService, IChatBotProvider chatterBot)
        {
            _chatterBot = chatterBot;
            _ruleService = ruleService;
        }
        public void HandleIncomeMessage(string source, string message, Action<string, string> responseAction)
        {
            Debug.WriteLine("Message received from {0}: {1}", source, message);

            Match chatBotMatch = Regex.Match(message.Trim(), @"^bot,(.*)");
            if (chatBotMatch.Success)
            {
                string messageForBot = chatBotMatch.Groups[1].Value;
                if (!string.IsNullOrEmpty(messageForBot))
                {
                    string chatBotResponse = _chatterBot.Think(messageForBot);
                    if (!string.IsNullOrEmpty(chatBotResponse))
                    {
                        responseAction(source, chatBotResponse.Trim());
                    }
                }
            }
            
            string ruleServiceResponse = _ruleService.GetApplicableRuleResult(message);
            if(!string.IsNullOrEmpty(ruleServiceResponse))
            {
                responseAction(source, ruleServiceResponse);
            }
        }
    }
}
