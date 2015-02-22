using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using SQLite;

namespace SkypeBot.SkypeDB.SkypeDalImplementations
{
    public class SkypeDal7: ISkypeDal
    {
        private string _dbCopyFilePath;

        #region helpers
        private void CopyTempDb(string accountName)
        {
            string appDatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dbOriginalFile = string.Format("{0}\\Skype\\{1}\\main.db", appDatadir, accountName);
            _dbCopyFilePath = string.Format("{0}\\Skype\\{1}\\main_copy.db", appDatadir, accountName);
            if (File.Exists(dbOriginalFile))
            {
                File.Copy(dbOriginalFile, _dbCopyFilePath, true);
            }
        }

        private void RemoveTempDb()
        {
            if (File.Exists(_dbCopyFilePath))
            {
                File.Delete(_dbCopyFilePath);
            }
        }
        private List<T> GetList<T>(string sql)
        {
            using (var connection = new SQLiteConnection(_dbCopyFilePath, SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand(sql);
                return command.ExecuteQuery<T>();
            }
        }
        #endregion
        public SkypeDal7()
        {
            CopyTempDb(ConfigurationManager.AppSettings["botSkypeName"]);
        }

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
            return GetList<SkypeMessage>("select * from messages where body_xml like '%" + filter + "%'");
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
                GetList<SkypeMessage>(string.Format("select * from messages where convo_id={0} and id>{1} order by id",
                    conversationId, lastMessageId));
        }

        public void Dispose()
        {
            RemoveTempDb();
        }
    }
}
