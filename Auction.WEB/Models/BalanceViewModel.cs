using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.WEB.Models
{
    public class BalanceViewModel
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal AddMoney { get; set; }
    }
}