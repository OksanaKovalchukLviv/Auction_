using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool GetLetters { get; set; }

        public decimal Balance { get; set; }

        public decimal FrozenBalance { get; set; }
    }
}
