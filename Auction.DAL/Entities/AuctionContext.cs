using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Auction.DAL.Entities
{
    public partial class AuctionContext : DbContext
    {
        public AuctionContext()
            : base("name=AuctionDataModel")
        {
        }

        public AuctionContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lot>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.Lot)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Lots)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);
        }
    }
}
