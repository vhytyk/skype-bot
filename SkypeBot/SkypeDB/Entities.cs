using System;
using System.Collections.Generic;
using SQLite;

namespace SkypeBot.SkypeDB
{
    public class SkypeContact
    {
        [PrimaryKey]
        public long Id { get; set; }
        [Column("skypename")]
        public string Name { get; set; }
        [Column("displayname")]
        public string DisplayName { get; set; }
        [Column("isauthorized")]
        public bool IsAuthorized { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", DisplayName);
        }
    }
    public class SkypeChat
    {
        public long Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("friendlyname")]
        public string DisplayName { get; set; }
        [Column("dialog_partner")]
        public string DialogPartner { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", DisplayName);
        }
    }

    public class SkypeConversation
    {
        [PrimaryKey, Column("id")]
        public long Id { get; set; }
        [Column("identity")]
        public string Name { get; set; }
        [Column("displayname")]
        public string DisplayName { get; set; }
        public override string ToString()
        {
            return string.Format("{0}", DisplayName);
        }
    }

    public enum MessageType {Message = 61, LeaveTheChat = 13, EnterTheChat = 10}

    public class SkypeSearchResult
    {
        public string ChatName { get; set; }
        public List<SkypeMessage> Messages { get; set; }
    }

    public class SkypeMessage
    {
        [PrimaryKey, Column("id")]
        public long Id { get; set; }
        [Column("body_xml")]
        public string Message { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("from_dispname")]
        public string AuthorDisplayName { get; set; }
        [Column("identities")]
        public string PointedPerson { get; set; }
        [Column("type")]
        public MessageType Type { get; set; }
        [Column("timestamp")]
        public double Timestamp { get; set; }
        [Column("convo_id")]
        public int ConversationId { get; set; }

        public DateTime Date { get { return Timestamp.UnixTimeStampToDateTime(); } }

        public override string ToString()
        {
            return string.Format("[{0}] {1}: {2}",Date, Author, Message);
        }
    }
}
