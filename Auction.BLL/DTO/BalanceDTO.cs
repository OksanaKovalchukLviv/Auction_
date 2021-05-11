using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.DTO
{
    public class BalanceDTO
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal AddMoney { get; set; }
    }
}
