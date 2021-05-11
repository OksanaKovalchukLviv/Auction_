using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.DTO
{
    public class LotDTO
    {
        public int LotId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal InitialPrice { get; set; }

        public int LotTime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public int CreatedBy { get; set; }

        public int Status { get; set; }

        public List<BidDTO> BidsForLot { get; set; }
    }
}
