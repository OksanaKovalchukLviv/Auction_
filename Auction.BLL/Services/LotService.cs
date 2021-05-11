using Auction.BLL.DTO;
using Auction.BLL.Infrastructure;
using Auction.BLL.Interfaces;
using Auction.Common;
using Auction.DAL.Entities;
using Auction.DAL.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Auction.BLL.Services
{
    public class LotService : IService<LotDTO>
    {
        public LotService()
        {
            efu = new EFUnitOfWork(Configuration.cs);
        }

        public LotService(string connectionString)
        {
            efu = new EFUnitOfWork(connectionString);
        }

        EFUnitOfWork efu;
        public BLLMethodResult CreateLot(LotDTO lotFromView, int userId)
        {
            BLLMethodResult result = new BLLMethodResult();
                try
                {
                    if (TimeCondition(lotFromView.LotTime))
                    {
                        Lot lot = new Lot();

                        lot.Name = lotFromView.Name;
                        lot.Description = lotFromView.Description;
                        lot.InitialPrice = lotFromView.InitialPrice;
                        lot.ExpirationTime = DateTime.Now.AddMinutes(lotFromView.LotTime);
                        lot.CreatedBy = userId;
                        lot.CreatedDateTime = DateTime.Now;
                        lot.ModifiedBy = userId;
                        lot.ModifiedDateTime = DateTime.Now;

                        lot.Status = (int)Enum.LotStatus.Created;

                        efu.Lots.Create(lot);

                        result.Result = 0;
                        result.Message = "Your lot has been added.";
                    }
                    else
                    {
                    result.Result = 2;
                    result.Message = "Time is outside the allowable limits.";
                    }
                }
                catch (ValidationException ex)
                {
                    result.Result = 1;
                    result.Message = ex.Message;
                }
            
            return result;
        }

        private bool TimeCondition(int time)
        {
            if (time >= 5 && time <= 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<LotDTO> AllLotsFromDB()
        {
            try
            {
                IEnumerable<Lot> lotsFromDB = (IEnumerable<Lot>)efu.Lots.GetAll();
                IEnumerable<Lot> lotsSort = lotsFromDB.OrderByDescending(x => x.LotId);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotDTO>());
                var mapper = new Mapper(config);
                var lots = mapper.Map<List<LotDTO>>(lotsSort);

                return lots;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public LotDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BidDTO> AllBidsForLot(int lotId)
        {
            try
            {
                IEnumerable<Bid> bidsFromDB = (IEnumerable<Bid>)efu.Bids.Find(x => x.LotId == lotId);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Bid, BidDTO>());
                var mapper = new Mapper(config);
                var bids = mapper.Map<List<BidDTO>>(bidsFromDB);
                return bids;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<LotDTO> LotsOfUserFromDB(int userId)
        {
            try
            {
                IEnumerable<Lot> lotsFromDB = (IEnumerable<Lot>)efu.Lots.Find(x => x.CreatedBy == userId);
                IEnumerable<Lot> lotsSort = lotsFromDB.OrderByDescending(x => x.LotId);
                var config = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotDTO>());
                var mapper = new Mapper(config);
                var lots = mapper.Map<List<LotDTO>>(lotsSort);
                return lots;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BLLMethodResult UpdateLot(LotDTO lotDTO)
        {
            BLLMethodResult result = new BLLMethodResult();
            try
            {
                IEnumerable<BidDTO> bidsForLot = AllBidsForLot(lotDTO.LotId);
                if (bidsForLot.Count() == 0)
                {
                    if (TimeCondition(lotDTO.LotTime))
                    {
                        Lot lot = efu.Lots.Get(lotDTO.LotId);

                        lot.Name = lotDTO.Name;
                        lot.Description = lotDTO.Description;
                        lot.InitialPrice = lotDTO.InitialPrice;
                        lot.ExpirationTime = DateTime.Now.AddMinutes(lotDTO.LotTime);
                        lot.ModifiedBy = lotDTO.CreatedBy;
                        lot.ModifiedDateTime = DateTime.Now;

                        efu.Lots.Update(lot);
                        efu.Lots.Save();

                        var config = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotDTO>());
                        var mapper = new Mapper(config);
                        LotDTO newlotDTO = mapper.Map<LotDTO>(lot);

                        result.Result = 0;
                        result.Message = "Your lot has been updated";
                        result.Object = newlotDTO;
                    }
                    else
                    {
                        result.Result = 2;
                        result.Message = "Time is outside the allowable limits.";
                    }
                }
                else
                {
                    result.Result = 1;
                    result.Message = "The lot has bids. It cannot be updated.";
                }
            }
            catch (Exception ex)
            {
                result.Result = 2;
                result.Message = ex.Message;
            }
            return result;
        }

        public BLLMethodResult DeleteLot(LotDTO lotDTO)
        {
            BLLMethodResult result = new BLLMethodResult();
            try
            {
                IEnumerable<BidDTO> bidsForLot = AllBidsForLot(lotDTO.LotId);
                if (bidsForLot.Count() == 0)
                {
                    Lot lot = efu.Lots.Get(lotDTO.LotId);
                    efu.Lots.Delete(lot.LotId);
                    result.Result = 0;
                    result.Message = "Your lot has been deleted";
                }
                else
                {
                    result.Result = 1;
                    result.Message = "The lot has bids. It cannot be deleted.";
                }
            }
            catch (Exception ex)
            {
                result.Result = 2;
                result.Message = ex.Message;
            }
            return result;
        }

        public IEnumerable<LotDTO> LotsForBids(int lotId)
        {
            try
            {
                IEnumerable<Lot> lotsFromDB = (IEnumerable<Lot>)efu.Lots.Find(x => x.CreatedBy != lotId && x.ExpirationTime > DateTime.Now);
                IEnumerable<Lot> lotsSort = lotsFromDB.OrderByDescending(x => x.LotId);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Lot, LotDTO>());
                var mapper = new Mapper(config);
                List<LotDTO> lots = mapper.Map<List<LotDTO>>(lotsSort);

                return lots;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BidWinnerDTO LotWinner(int lotId)
        {
            IEnumerable<Bid> bids = efu.Bids.Find(x => x.LotId == lotId).OrderByDescending(x => x.CreatedDateTime);
            Bid lastBid = bids.FirstOrDefault();

            if (lastBid != null)
            {
                UserService us = new UserService();
                User userWinner = efu.Users.Get(lastBid.CreatedBy);

                var config = new MapperConfiguration(cfg => cfg.CreateMap<Bid, BidWinnerDTO>());
                var mapper = new Mapper(config);
                var bidWinnerDTO = mapper.Map<BidWinnerDTO>(lastBid);

                bidWinnerDTO.Email = userWinner.Email;

                return bidWinnerDTO;
            }
            else
            {
                return null;
            }
        }

        public void ClosingLot()
        {
            IEnumerable<Lot> lotsForClosing = efu.Lots.Find(x => x.Status == (int)Enum.LotStatus.Created  && x.ExpirationTime < DateTime.Now);

            foreach (Lot lot in lotsForClosing)
            {
                Bid lastBid = efu.Bids.Find(x => x.LotId == lot.LotId).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();

                if (lastBid != null)
                {
                    User userWinner = efu.Users.Get(lastBid.CreatedBy);
                    userWinner.FrozenBalance = userWinner.FrozenBalance - lastBid.BidPrice;
                    efu.Users.Update(userWinner);

                    Lot lotForCreator = efu.Lots.Get(lot.LotId);
                    User userCreator = efu.Users.Get(lotForCreator.CreatedBy);
                    userCreator.Balance = userCreator.Balance + lastBid.BidPrice;

                    lot.Status = (int)Enum.LotStatus.Finished;
                    efu.Lots.Update(lot);

                    SendEmail(lot.LotId);
                }
                else
                {
                    lot.Status = (int)Enum.LotStatus.Finished;
                    efu.Lots.Update(lot);
                }
            }
            efu.Users.Save();
            efu.Lots.Save();
        }

        public void SendEmail(int lotId)
        {
            Lot lot = efu.Lots.Get(lotId);
            User userOwner = efu.Users.Get(lot.CreatedBy);
            IEnumerable<Bid> bidsFromDB = efu.Bids.Find(x => x.LotId == lotId);
            Bid lastBid = bidsFromDB.OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();
            User userWinner = efu.Users.Get(lastBid.CreatedBy);

            if (userOwner.GetLetters == true)
            {
                MailAddress from1 = new MailAddress(Configuration.Email, "Auction");
                MailAddress to1 = new MailAddress(userOwner.Email);
                MailMessage m1 = new MailMessage(from1, to1);

                m1.Subject = "Your lot is closed";
                m1.Body = "<p>Your lot is closed. Information about lot:</p>";
                m1.Body += "<p>Name: " + lot.Name + "</p>";
                m1.Body += "<p>Description: " + lot.Description + "</p>";
                m1.Body += "<p>Initial Price: " + lot.InitialPrice + "</p>";
                m1.Body += "<p>Last Price: " + lastBid.BidPrice + "Time: " + lastBid.CreatedDateTime + " </p>";
                m1.Body += "<p>History of bids:</p>";

                foreach (Bid bid in bidsFromDB)
                {
                    m1.Body += "<p>" + bid.BidPrice + "       " + bid.CreatedDateTime + "</p>";
                }
                m1.IsBodyHtml = true;

                AuctionSmtpClient.Send(m1);         
            }

            if (userWinner.GetLetters == true)
            {
                MailAddress from = new MailAddress(Configuration.Email, "Auction");
                MailAddress to = new MailAddress(userWinner.Email);
                MailMessage m = new MailMessage(from, to);

                m.Subject = "You won the lot";
                m.Body = "<p>You won the lot. Information about lot:</p>";
                m.Body += "<p>Name: " + lot.Name + "</p>";
                m.Body += "<p>Description: " + lot.Description + "</p>";
                m.Body += "<p>Initial Price: " + lot.InitialPrice + "</p>";
                m.Body += "<p>Your Price: " + lastBid.BidPrice + "Time: " + lastBid.CreatedDateTime + " </p>";
                m.Body += "<p>The money has been debited from your account.</p>";
                m.IsBodyHtml = true;

                AuctionSmtpClient.Send(m);
            }
        }
    }
}