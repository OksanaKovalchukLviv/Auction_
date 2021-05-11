using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Auction.WEB.Models;
using System;
using System.Collections.Generic;
using Auction.BLL.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Auction.DAL.Repositories;
using Auction.BLL.DTO;
using AutoMapper;
using Auction.BLL.Services;
using Auction.WEB.AuthData;
using Auction.Common;

namespace Auction.WEB.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserViewModel userFromView)
        {
            UserService us = new UserService();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserViewModel, UserDTO>());
            var mapper = new Mapper(config);
            var userDTO = mapper.Map<UserDTO>(userFromView);

            BLLMethodResult result = us.CreateUser(userDTO);

            return RedirectToAction("Message", "Home", new { str = result.Message });
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult Balance()
        {
            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            UserService us = new UserService();
            BalanceDTO balanceDTO = us.UserToAddBalance(userId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<BalanceDTO, BalanceViewModel>());
            var mapper = new Mapper(config);
            var balanceModelView = mapper.Map<BalanceViewModel>(balanceDTO);

            return View(balanceModelView);
        }

        [AuthAttribute]
        [HttpPost]
        public ActionResult Balance(BalanceViewModel balanceFromView)
        {
            UserService us = new UserService();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BalanceViewModel, BalanceDTO>());
            var mapper = new Mapper(config);
            var balanceDTO = mapper.Map<BalanceDTO>(balanceFromView);
            BLLMethodResult result = us.AddToBalance(balanceDTO);
            return RedirectToAction("Message", "Home", new { str = result.Message });
        }
    }
}