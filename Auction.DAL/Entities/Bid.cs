namespace Auction.DAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bid")]
    public partial class Bid
    {
        public int BidId { get; set; }

        public int LotId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public decimal BidPrice { get; set; }

        public virtual User User { get; set; }

        public virtual Lot Lot { get; set; }
    }
}
