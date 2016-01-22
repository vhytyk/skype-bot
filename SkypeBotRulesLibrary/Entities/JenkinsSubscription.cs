using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SQLite;

namespace SkypeBotRulesLibrary.Entities
{
    [Table("JenkinsSubscriptions")]
    public class JenkinsSubscription
    {
        [PrimaryKey, Column("Id"), AutoIncrement]
        public int Id { get; set; }

        [Column("ConversationName")]
        [Description("Conversation name")]
        public string ConversationName { get; set; }

        [Column("JenkisJobName")]
        [Description("Jenkis job name")]
        public string JenkisJobName { get; set; }

        [Column("Active")]
        public bool Active { get; set; }
    }
}
