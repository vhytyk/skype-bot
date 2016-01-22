using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeBotRulesLibrary
{
    public interface IUniversalDal<T> 
    {
        List<T> GetAll();
        bool Add(T newItem);
        bool Update(T newItem);
        T GetById(int id);
        bool Delete(int id);
        bool Delete(T item);
        int DeleteAll();
        void DropTable();
        void CreateTable(); 
    }
}
