using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SQLite;

namespace SkypeBotRulesLibrary.Implementations
{
    public class BaseDal
    {
        protected string _dbFilePath;
        public BaseDal()
        {
            _dbFilePath = ConfigurationManager.AppSettings["DBPath"];
            InitDb();
        }

        protected virtual void InitDb()
        {
            //e.g. create table if not exists
        }

        #region helpers

        protected List<T> GetList<T>(string sql)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand(sql);
                return command.ExecuteQuery<T>();
            }
        }
        #endregion
    }
}
