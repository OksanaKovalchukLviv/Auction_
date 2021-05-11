using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.WEB.Models
{
    public class LotViewModel
    {
        public int LotId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal InitialPrice { get; set; }

        public int LotTime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public int CreatedBy { get; set; }

        public int Status { get; set; }

        public List<BidsForLotViewModel> BidsForLot { get; set; }
    }
}