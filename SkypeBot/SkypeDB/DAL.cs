using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SQLite;
using System.Configuration;

namespace SkypeCore
{
    public class SkypeDAL
    {
        public string AccountName { get; set; }

        public SkypeDAL(string accountName)
        {
            AccountName = accountName;
        }

        #region helpers
        private List<T> GetList<T>(string sql)
        {
            using (var connection = new SQLiteConnection(Utils.CopyDb(AccountName), SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand(sql);
                return command.ExecuteQuery<T>();
            } 
        }
        #endregion

        public List<SkypeContact> GetAllContacts()
        {
            return GetList<SkypeContact>("select * from contacts");
        }

        public List<SkypeChat> GetAllChats()
        {
            return GetList<SkypeChat>("select * from chats");
        }

        public List<SkypeConversation> GetAllConversations()
        {
            return GetList<SkypeConversation>("select * from conversations");
        }

        public List<SkypeMessage> GetAllMessagesContains(string filter)
        {
            return GetList<SkypeMessage>("select * from messages where body_xml like '%" + filter+"%'");
        }

        public List<SkypeMessage> GetAllMessagesContains(string[] filter)
        {
            string condition = string.Join(" and ",
                filter.Where(f => !String.IsNullOrWhiteSpace(f)).Select(f => string.Format("LOWER(body_xml) like '%{0}%'", f.ToLower())));
            if (!String.IsNullOrWhiteSpace(condition))
            {
                return GetList<SkypeMessage>(string.Format("select * from messages where {0}", condition));
            }
            return new List<SkypeMessage>();
        }

        public List<SkypeMessage> GetAllMessages()
        {
            return GetList<SkypeMessage>("select * from messages");
        }

        public List<SkypeMessage> GetLastMessages(long lastMessageId, long conversationId)
        {
            return
                GetList<SkypeMessage>(string.Format("select * from messages where convo_id={0} and id>{1}",
                    conversationId, lastMessageId));
        }

    }
}
