using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private AuctionContext db;

        public Repository(AuctionContext context)
        {
            this.db = context;
        }

        public void Create(T item)
        {
            db.Set<T>().Add(item);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            T item = db.Set<T>().Find(id);
            db.Set<T>().Remove(item);
            //db.Entry(item).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return db.Set<T>().Where(predicate);
        }

        public T Get(int id)
        {
            T item = db.Set<T>().Find(id);
            return item;
        }

        public IEnumerable<T> GetAll()
        {
            return db.Set<T>().Select(row => row).ToList<T>();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(T item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
