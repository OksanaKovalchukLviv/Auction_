using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.WEB.Models
{
    public class BidViewModel
    {
        public int BidId { get; set; }

        public int LotId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal InitialPrice { get; set; }

        public DateTime ExpirationTime { get; set; }

        public decimal BidPrice { get; set; }

        
    }
}