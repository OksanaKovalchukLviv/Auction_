using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Auction.WEB.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool GetLetters { get; set; }
    }
}