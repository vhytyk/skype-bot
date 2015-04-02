using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SkypeBotRulesLibrary.Entities;
using SkypeBotRulesLibrary.Interfaces;
using SQLite;

namespace SkypeBotRulesLibrary.Implementations
{
    public class SkypeNameDal : BaseDal, ISkypeNameDal
    {
        protected override void InitDb()
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite))
            {
                if (connection.GetTableInfo("SkypeNames").Count == 0)
                {
                    connection.CreateTable<SkypeNameInfo>();
                }
            }
        }

        public List<SkypeNameInfo> GetAll()
        {
            return GetList<SkypeNameInfo>("select * from SkypeNames");
        }

        public bool Add(SkypeNameInfo skypeNameInfo)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite))
            {
                SQLiteCommand command = connection.CreateCommand("insert into SkypeNames(SkypeName, JaDomainName) values (?,?)"
                    , skypeNameInfo.SkypeName
                    , skypeNameInfo.JaDomainName);
                return command.ExecuteNonQuery() > 0;
            }
        }

        public SkypeNameInfo GetById(int id)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand(string.Format("select * from SkypeNames where Id={0}", id));
                return command.ExecuteQuery<SkypeNameInfo>().FirstOrDefault();
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadWrite))
            {
                SQLiteCommand command = connection.CreateCommand("delete from SkypeNames where Id=?", id);
                return command.ExecuteNonQuery() > 0;
            }
        }


        public SkypeNameInfo GetByDomainName(string domainName)
        {
            using (var connection = new SQLiteConnection(_dbFilePath, SQLiteOpenFlags.ReadOnly))
            {
                SQLiteCommand command = connection.CreateCommand("select * from SkypeNames where JaDomainName=?",
                    domainName);
                
                return command.ExecuteQuery<SkypeNameInfo>().FirstOrDefault();
            }
        }
    }
}
