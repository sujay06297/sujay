using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebClass.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private AOnlineClassEntities db = null;
        //空的AOnlineClassEntities，建立的時候從Controler宣告
        private DbSet<T> Dbset = null;


        public Repository() //建構函式
        {
            db = new AOnlineClassEntities();
            Dbset = db.Set<T>();
        }

        public void Create(T _entity)
        {
            Dbset.Add(_entity);
            db.SaveChanges();
        }

        public void Delete(T _entity)
        {
            Dbset.Remove(_entity);
            db.SaveChanges();
        }

        public IEnumerable<T> GetAll()
        {
            return Dbset.ToList();
        }

        public T GetById(int id)
        {
            return Dbset.Find(id);
        }

        public void Update(T _entity)
        {
            db.Entry(_entity).State = EntityState.Modified;
            db.SaveChanges();

        }
    }
}