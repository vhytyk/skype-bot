using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBotRulesLibrary
{
    [Table("Rules")]
    public class SkypeBotRule
    {
        [PrimaryKey, Column("Id")]
        public long Id { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Rule")]
        public string Rule { get; set; }
        [Column("Value")]
        public string Value { get; set; }
        public override string ToString()
        {
            return string.Format("Id:{0}, Name:{1}, Rule:{2}, Value:{3}", Id, Name, Rule, Value);
        }
    }
}
