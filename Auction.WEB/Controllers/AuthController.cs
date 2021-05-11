using Auction.BLL.DTO;
using Auction.BLL.Infrastructure;
using Auction.BLL.Services;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using Auction.WEB.Models;
using Auction.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.WEB.Controllers
{
    public class AuthController : Controller
    {
        AuctionContext db = new AuctionContext();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AuthViewModel authFromView)
        {
            UserDTO userDTO = new UserDTO();
            UserService us = new UserService();

            userDTO.Email = authFromView.Email;
            userDTO.Password = authFromView.Password;

            BLLMethodResult result = us.LoginUser(userDTO);

            if (result.Result == 0)
            {
                Response.Cookies["UserId"].Value = userDTO.UserId.ToString();
                Response.Cookies["UserId"].Expires = DateTime.Now.AddHours(1);

                Response.Cookies["Email"].Value = userDTO.Email;
                Response.Cookies["Email"].Expires = DateTime.Now.AddHours(1);

                Session["UserEmail"] = userDTO.Email;

                return RedirectToAction("AllLots", "Lot");
            }
            else
            {
                return RedirectToAction("Message", "Home", new { str = result.Message });
            }
        }

        public ActionResult Logout()
        {
            Session["UserEmail"] = null;
            if (Request.Cookies["Email"] != null)
            {
                var c = new HttpCookie("Email");
                c.Expires = DateTime.Now;
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["UserId"] != null)
            {
                var c = new HttpCookie("UserId");
                c.Expires = DateTime.Now;
                Response.Cookies.Add(c);
            }
            Session.Abandon();
            return RedirectToAction("AllLots", "Lot");
        }
    }  
}