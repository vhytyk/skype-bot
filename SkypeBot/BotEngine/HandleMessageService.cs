using System;
using System.Diagnostics;
using ChatterBotAPI;
using SkypeBot.SkypeDB;
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
        public void HandleIncomeMessage(string source, SkypeMessage message, Action<string, string> responseAction)
        {
            try
            {
                Match chatBotMatch = Regex.Match(message.Message.Trim(), @"^bot,(.*)");
                if (chatBotMatch.Success)
                {
                    string messageForBot = chatBotMatch.Groups[1].Value;
                    if (!string.IsNullOrEmpty(messageForBot))
                    {
                        string chatBotResponse = _chatterBot.Think(messageForBot);
                        if (!string.IsNullOrEmpty(chatBotResponse))
                        {
                            chatBotResponse = string.Format("@{0}, {1}", message.AuthorDisplayName, chatBotResponse);
                            responseAction(source, chatBotResponse.Trim());
                        }
                    }
                }

                string ruleServiceResponse = _ruleService.GetApplicableRuleResult(message.Message);
                if (!string.IsNullOrEmpty(ruleServiceResponse))
                {
                    responseAction(source, ruleServiceResponse);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("HandleInclomeMessage error:\r\nsource:{0}\r\nmessage:{1}\r\nerror:{2}", source, message.ToString(), ex.Message);
            }
        }
    }
}
