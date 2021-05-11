using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private AuctionContext db;

        public Repository<User> Users;
        public Repository<Lot> Lots;
        public Repository<Bid> Bids;

        public EFUnitOfWork(string connectionString)
        {
            db = new AuctionContext(connectionString);

            Users = new Repository<User>(db);
            Lots = new Repository<Lot>(db);
            Bids = new Repository<Bid>(db);

        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
