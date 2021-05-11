using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Common
{
    public class Configuration
    {
        public static string cs = ConfigurationManager.ConnectionStrings["AuctionDataModel"].ConnectionString;

        public static string Email = "email@gmail.com";

        public static string password = "password";
    }
}