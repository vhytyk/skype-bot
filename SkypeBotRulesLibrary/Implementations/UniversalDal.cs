using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SkypeBotRulesLibrary.Implementations
{
    public class UniversalDal<T> : BaseDal, IUniversalDal<T> where T : new()
    {
        public UniversalDal()
        {
            CreateTable();
        }

        public void CreateTable()
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                if (db.GetTableInfo(db.GetMapping<T>().TableName).Count == 0)
                {
                    db.CreateTable<T>();
                }
            }
        }
        public List<T> GetAll()
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Table<T>().ToList();
            }
        }

        public bool Add(T newItem)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Insert(newItem) > 0;
            }
        }

        public bool Update(T newItem)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Update(newItem) > 0;
            }
        }

        public T GetById(int id)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Get<T>(id);
            }
        }

        public bool Delete(int id)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Delete<T>(id) > 0;
            }
        }

        public bool Delete(T item)
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.Delete(item) > 0;
            }
        }

        public int DeleteAll()
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                return db.DeleteAll<T>();
            }
        }

        public void DropTable()
        {
            using (SQLiteConnection db = new SQLiteConnection(_dbFilePath))
            {
                if (db.GetTableInfo(db.GetMapping<T>().TableName).Count > 0)
                {
                    db.DropTable<T>();
                }
            }
        }
    }
}
