using System;
using System.Collections.Generic;

namespace SkypeBot.SkypeDB
{
    public interface ISkypeDal : IDisposable
    {
        List<SkypeContact> GetAllContacts();
        List<SkypeChat> GetAllChats();
        List<SkypeConversation> GetAllConversations();
        List<SkypeMessage> GetAllMessagesContains(string filter);
        List<SkypeMessage> GetAllMessagesContains(string[] filter);
        List<SkypeMessage> GetAllMessages();
        List<SkypeMessage> GetLastMessages(long lastMessageId, long conversationId);
    }
}
