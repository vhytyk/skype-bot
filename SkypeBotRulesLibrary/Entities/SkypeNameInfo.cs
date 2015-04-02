using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace SkypeBotRulesLibrary.Entities
{
    [Table("SkypeNames")]
    public class SkypeNameInfo
    {
        [PrimaryKey, Column("Id"), AutoIncrement]
        public int Id { get; set; }
        [Column("SkypeName")]
        public string SkypeName { get; set; }
        [Column("JaDomainName")]
        public string JaDomainName { get; set; }
    }
}
