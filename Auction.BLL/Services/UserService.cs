using Auction.BLL.DTO;
using Auction.BLL.Infrastructure;
using Auction.BLL.Interfaces;
using Auction.Common;
using Auction.DAL.Entities;
using Auction.DAL.Interfaces;
using Auction.DAL.Repositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Auction.BLL.Services
{
    public class UserService: IService<UserDTO>
    {
        EFUnitOfWork efu = new EFUnitOfWork(Configuration.cs);
        public BLLMethodResult CreateUser(UserDTO userFromView)
        {
            BLLMethodResult result = new BLLMethodResult();
            bool flag = true;

            IEnumerable<User> users = efu.Users.GetAll();

            foreach (User user in users)
            {
                if (user.Email == userFromView.Email)
                {
                    result.Result = 2;
                    result.Message = "A user with this name is already registered";
                    flag = false;
                    break;
                }
            }

            if (userFromView.Password != userFromView.ConfirmPassword)
            {
                result.Result = 3;
                result.Message = "Password and confirm password are different.";
                flag = false;
            }

            if (flag)
            {
                try
                {
                    User user = new User();

                    user.Email = userFromView.Email;
                    user.Password = userFromView.Password;
                    user.Balance = 0;
                    user.FrozenBalance = 0;
                    user.GetLetters = userFromView.GetLetters;
                    user.CreatedBy = 0;
                    user.CreatedDateTime = DateTime.Now;
                    user.ModifiedBy = 0;
                    user.ModifiedDateTime = DateTime.Now;

                    efu.Users.Create(user);

                    result.Result = 0;
                    result.Message = "Your registration is successful";
                }
                catch (ValidationException ex)
                {
                    result.Result = 1;
                    result.Message = ex.Message;
                }
            }
            return result;
        }

        public BLLMethodResult LoginUser(UserDTO authFromView)
        {
            BLLMethodResult result = new BLLMethodResult();

            try
            {
                User user = efu.Users.Find(x => x.Email == authFromView.Email && x.Password == authFromView.Password).FirstOrDefault();

                if (user != null)
                {
                    authFromView.UserId = user.UserId;
                    result.Result = 0;
                    result.Message = "Your login is successful";
                }
                else
                {
                    result.Result = 2;
                    result.Message = "There is a mistake in login or in password.";
                }
            }
            catch (Exception ex)
            {
                result.Result = 1;
                result.Message = ex.Message;
            }

            return result;
        }

        public BLLMethodResult AddToBalance(BalanceDTO balanceDTO)
        {
            BLLMethodResult result = new BLLMethodResult();

            try
            {
                User user = efu.Users.Get(balanceDTO.UserId);
                user.Balance = user.Balance + balanceDTO.AddMoney;

                efu.Users.Update(user);
                efu.Users.Save();

                result.Result = 0;
                result.Message = "Your balance is update";
            }
            catch (Exception ex)
            {
                result.Result = 1;
                result.Message = ex.Message;
            }
            return result;
        }

        public BalanceDTO UserToAddBalance(int userId)
        {
            User user = efu.Users.Get(userId);
            BalanceDTO balanceDTO = new BalanceDTO();

            balanceDTO.UserId = userId;
            balanceDTO.Balance = user.Balance;

            return balanceDTO;
        }

        public UserDTO Get(int id)
        {
            User user = efu.Users.Get(id);
            
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>());
            var mapper = new Mapper(config);
            UserDTO userDTO = mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public BLLMethodResult AddToFrozenBalance(UserDTO userDTO)
        {
            BLLMethodResult result = new BLLMethodResult();
            try
            {
                User user = efu.Users.Get(userDTO.UserId);

                user.Balance = userDTO.Balance;
                user.FrozenBalance = user.FrozenBalance + userDTO.FrozenBalance;

                efu.Users.Update(user);
                efu.Users.Save();

                result.Result = 0;
                result.Message = "Your bid has been accepted";
            }
            catch (Exception ex)
            {
                result.Result = 1;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
