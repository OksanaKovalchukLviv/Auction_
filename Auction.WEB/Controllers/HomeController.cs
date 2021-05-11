using Auction.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Message(string str)
        {
            MessageViewModel mv = new MessageViewModel();
            mv.Message = str;

            return View(mv);
        }
    }
}