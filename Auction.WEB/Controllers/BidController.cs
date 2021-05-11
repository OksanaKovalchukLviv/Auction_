using Auction.BLL.DTO;
using Auction.BLL.Services;
using Auction.Common;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using Auction.WEB.AuthData;
using Auction.WEB.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.WEB.Controllers
{
    public class BidController : Controller
    {
        [AuthAttribute]
        [HttpPost]
        public ActionResult MakeBid(BidViewModel bidViewModel)
        {
            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            BidService bs = new BidService();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BidViewModel, BidDTO>());
            var mapper = new Mapper(config);
            var bidDTO = mapper.Map<BidDTO>(bidViewModel);

            BLLMethodResult result = bs.CreateBid(bidDTO, userId);

            return RedirectToAction("Message", "Home", new { str = result.Message });
        }
    }
}