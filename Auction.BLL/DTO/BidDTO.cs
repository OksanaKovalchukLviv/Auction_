using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.DTO
{
    public class BidDTO
    {
        public int BidId { get; set; }

        public int LotId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public decimal BidPrice { get; set; }

        public decimal InitialPrice { get; set; }

    }
}
