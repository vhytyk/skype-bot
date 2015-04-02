using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SkypeBotRulesLibrary.Implementations;
using SQLite;
using System.Configuration;

namespace SkypeBotRulesLibrary.Fakes
{
    public class FakeRuleDal: BaseDal, IRuleDal
    {
        public List<SkypeBotRule> GetAllRules()
        {
            return GetList<SkypeBotRule>("select * from Rules");
        }

        public bool AddRule(SkypeBotRule newRule)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite))
            {
                SQLiteCommand command = connection.CreateCommand("insert into Rules(Name, Rule, Value) values (?,?,?)"
                    , newRule.Name
                    , newRule.Rule
                    , newRule.Value);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public SkypeBotRule GetById(int id)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand(string.Format("select * from Rules where Id={0}", id));
                return command.ExecuteQuery<SkypeBotRule>().FirstOrDefault();
            }
        }


        public bool DeleteRule(int id)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite))
            {
                SQLiteCommand command = connection.CreateCommand("delete from Rules where Id=?", id);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}
