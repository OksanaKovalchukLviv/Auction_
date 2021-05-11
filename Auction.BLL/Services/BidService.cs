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
    public class BidService:IService<BidDTO>
    {
        EFUnitOfWork efu = new EFUnitOfWork(Configuration.cs);

        public BLLMethodResult CreateBid(BidDTO bidFromView, int userId)
        {
            BLLMethodResult result = new BLLMethodResult();

            try
            {
                UserService us = new UserService();
                UserDTO userDTO = us.Get(userId);

                if (bidFromView.BidPrice < userDTO.Balance)
                {

                    LotService ls = new LotService();
                    BidDTO lastBid = ls.AllBidsForLot(bidFromView.LotId).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();

                    if (bidFromView.BidPrice <= bidFromView.InitialPrice)
                    {
                        result.Result = 1;
                        result.Message = "Your bid is less than initial price.";
                    }
                    else if ((lastBid != null) && (bidFromView.BidPrice <= lastBid.BidPrice))
                    {
                        result.Result = 2;
                        result.Message = "Your bid is less than previous one.";
                    }
                    else
                    {
                        if (lastBid != null)
                        {
                            
                            int previousUserId = lastBid.CreatedBy;
                            int previousBidId = lastBid.BidId;

                            Bid previousBidForEmail = efu.Bids.Get(previousBidId);

                            var config = new MapperConfiguration(cfg => cfg.CreateMap<Bid, BidDTO>());
                            var mapper = new Mapper(config);
                            BidDTO previousBidForEmailDTO = mapper.Map<BidDTO>(previousBidForEmail);

                            User previousUser = efu.Users.Get(previousUserId);
                            previousUser.FrozenBalance = previousUser.FrozenBalance - lastBid.BidPrice;
                            previousUser.Balance = previousUser.Balance + lastBid.BidPrice;

                            efu.Users.Update(previousUser);
                            efu.Users.Save();

                            result = CreateBidWithPositiveResult(bidFromView, userId);

                            SendEmailAboutHigherBid(previousBidForEmailDTO);
                        }
                        else
                        {
                            result = CreateBidWithPositiveResult(bidFromView, userId);
                        }
                    }
                }
                else
                {
                    result.Result = 1;
                    result.Message = "Not enough money in the account. Please top up your account.";
                }
            }
            catch (ValidationException ex)
            {
                result.Result = 1;
                result.Message = ex.Message;
            }

            return result;
        }

        public BLLMethodResult CreateBidWithPositiveResult(BidDTO bidFromView, int userId)
        {
            BLLMethodResult result = new BLLMethodResult();

            UserService us = new UserService();
            UserDTO userDTO = us.Get(userId);

            userDTO.FrozenBalance = bidFromView.BidPrice;
            userDTO.Balance = userDTO.Balance - bidFromView.BidPrice;
            BLLMethodResult addBalanceToDB = us.AddToFrozenBalance(userDTO);

            Bid bidNew = new Bid();
            bidNew.LotId = bidFromView.LotId;
            bidNew.CreatedBy = userId;
            bidNew.CreatedDateTime = DateTime.Now;
            bidNew.BidPrice = bidFromView.BidPrice;

            efu.Bids.Create(bidNew);

            result.Result = 0;
            result.Message = "Your bid is in database";

            return result;
        }

        public BidDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public void SendEmailAboutHigherBid(BidDTO previousBid)
        {
            Bid newBid = efu.Bids.Find(x => x.LotId == previousBid.LotId).OrderByDescending(x => x.CreatedDateTime).FirstOrDefault();

            int previousUserId = previousBid.CreatedBy;
            User previousUser = efu.Users.Get(previousUserId);

            int lotId = previousBid.LotId;
            Lot lot = efu.Lots.Get(lotId);

            if (previousUser.GetLetters == true)
            {
                MailAddress from = new MailAddress(Configuration.Email, "Auction");
                MailAddress to = new MailAddress(previousUser.Email);
                MailMessage m = new MailMessage(from, to);

                m.Subject = "Higher bid";
                m.Body = "<p>There is a bid higher than yours for this lot:</p>";
                m.Body += "<p>Name: " + lot.Name + "</p>";
                m.Body += "<p>Description: " + lot.Description + "</p>";
                m.Body += "<p>Initial Price: " + lot.InitialPrice + "</p>";
                m.Body += "<p>Your Price: " + previousBid.BidPrice + "Time: " + previousBid.CreatedDateTime + " </p>";
                m.Body += "<p>New Price: " + newBid.BidPrice + "Time: " + newBid.CreatedDateTime + " </p>";
                m.IsBodyHtml = true;

                AuctionSmtpClient.Send(m);
            }
        }

    }
}
