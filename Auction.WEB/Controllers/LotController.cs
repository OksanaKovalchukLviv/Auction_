using Auction.BLL.DTO;
using Auction.BLL.Infrastructure;
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
    public class LotController : Controller
    {
        [AuthAttribute]
        [HttpGet]
        public ActionResult NewLot()
        {
            return View();
        }

        [AuthAttribute]
        [HttpPost]
        public ActionResult NewLot(LotViewModel lotFromView)
        {
            LotDTO lotDTO = new LotDTO();
            LotService ls = new LotService();

            lotDTO.Name = lotFromView.Name;
            lotDTO.Description = lotFromView.Description;
            lotDTO.InitialPrice = lotFromView.InitialPrice;
            lotDTO.LotTime = lotFromView.LotTime;

            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            BLLMethodResult result = ls.CreateLot(lotDTO, userId);

            return RedirectToAction("Message", "Home", new { str = result.Message });
        }

        [HttpGet]
        public ActionResult AllLots()
        {
            LotService ls = new LotService();
            IEnumerable<LotDTO> allLotsDTO = ls.AllLotsFromDB();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<LotDTO, LotViewModel>());
            var mapper = new Mapper(config);
            var allLots = mapper.Map<IEnumerable<LotViewModel>>(ls.AllLotsFromDB());

            return View(allLots);
        }

        [AuthAttribute]
        [HttpPost]
        public ActionResult MakeBid(LotViewModel lotViewModel)
        {
            BidService bs = new BidService();
            BidViewModel bidViewModel = new BidViewModel();

            bidViewModel.LotId = lotViewModel.LotId;
            bidViewModel.Name = lotViewModel.Name;
            bidViewModel.Description = lotViewModel.Description;
            bidViewModel.InitialPrice = lotViewModel.InitialPrice;
            bidViewModel.ExpirationTime = lotViewModel.ExpirationTime;

            return View(bidViewModel);
        }

        [AuthAttribute]
        public ActionResult BidsForLot(int lotId)
        {
            LotService ls = new LotService();
            LotDTO lotDto = new LotDTO();

            lotDto.BidsForLot = (List<BidDTO>)ls.AllBidsForLot(lotId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<BidDTO, BidsForLotViewModel>());
            var mapper = new Mapper(config);
            var bidsForLot = mapper.Map<List<BidsForLotViewModel>>(lotDto.BidsForLot);

            return View(bidsForLot);
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult LotsOfUser()
        {
            LotService ls = new LotService();

            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            IEnumerable<LotDTO> lotsDTO = ls.LotsOfUserFromDB(userId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<LotDTO, LotViewModel>());
            var mapper = new Mapper(config);
            var allLotsOfUser = mapper.Map<IEnumerable<LotViewModel>>(lotsDTO);

            return View(allLotsOfUser);
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult LotInformation(LotViewModel lotViewModel)
        {
            LotService bs = new LotService();
            LotDTO lotDto = new LotDTO();

            lotDto.BidsForLot = (List<BidDTO>)bs.AllBidsForLot(lotViewModel.LotId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<BidDTO, BidsForLotViewModel>());
            var mapper = new Mapper(config);
            var bidsForLot = mapper.Map<List<BidsForLotViewModel>>(lotDto.BidsForLot);

            lotViewModel.BidsForLot = (List<BidsForLotViewModel>)bidsForLot;
            return View(lotViewModel);
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult UpdateLot(LotViewModel lotViewModel)
        {
            return View(lotViewModel);
        }

        [AuthAttribute]
        [HttpPost]
        public ActionResult UpdateLotPost(LotViewModel lotViewModel)
        {
            LotService ls = new LotService();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<LotViewModel, LotDTO>());
            var mapper = new Mapper(config);
            var lotDTO = mapper.Map<LotDTO>(lotViewModel);

            BLLMethodResult result = ls.UpdateLot(lotDTO);
            if (result.Result == 0)
            {
                return RedirectToAction("LotInformation", "Lot", result.Object);
            }
            else
            {
                return RedirectToAction("Message", "Home", new { str = result.Message });
            }
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult DeleteLot(int lotId)
        {
            LotService ls = new LotService();

            BLLMethodResult result = ls.DeleteLot(lotId);

            if (result.Result == 0)
            {
                return RedirectToAction("LotsOfUser", "Lot");
            }
            else
            {
                return RedirectToAction("Message", "Home", new { str = result.Message });
            }
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult LotsForBids()
        {
            LotService ls = new LotService();

            int userId = Convert.ToInt32(Request.Cookies["UserId"].Value);

            IEnumerable<LotDTO> lotsForBidsDTO = ls.LotsForBids(userId);

            var config = new MapperConfiguration(cfg => cfg.CreateMap<LotDTO, LotViewModel>());
            var mapper = new Mapper(config);
            var allLots = mapper.Map<IEnumerable<LotViewModel>>(lotsForBidsDTO);

            return View("AllLots", allLots);
        }

        [AuthAttribute]
        [HttpGet]
        public ActionResult InformationAboutWinner(int lotId)
        {
            LotService ls = new LotService();
            BidWinnerDTO bidWinnerDTO = ls.LotWinner(lotId);
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BidWinnerDTO, BidWinnerViewModel>());
            var mapper = new Mapper(config);
            var bidWinner = mapper.Map<BidWinnerViewModel>(bidWinnerDTO);

            return View(bidWinner);
        }

        [HttpGet]
        public ActionResult Test()
        {
            LotService ls = new LotService();
            ls.ClosingLot();
            return View();
        }

    }
}