using Auction.BLL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Service
{
    class Program
    {
        static void Main(string[] args)
        {
           string connectionString = ConfigurationManager.ConnectionStrings["AuctionDataModel"].ConnectionString;

            LotService ls = new LotService(connectionString);
            ls.ClosingLot();
            Console.WriteLine("Everything is good!");
        }
    }
}
