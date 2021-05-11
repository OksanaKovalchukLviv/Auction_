using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.WEB.Models
{
    public class BidWinnerViewModel
    {
        public int BidId { get; set; }

        public int LotId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public decimal BidPrice { get; set; }

        public string Email { get; set; }
    }
}