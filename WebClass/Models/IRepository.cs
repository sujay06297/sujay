using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClass.Models
{
    interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Create(T _entity);
        void Update(T _entity);
        void Delete(T _entity);
    }
}
