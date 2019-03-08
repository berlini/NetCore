using DoTheDishesWebservice.Core.Utils;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace DoTheDishesWebservice.Core.Services
{
    public class UsersService : IUserService
    {
        private readonly IUserRespository UserRepository;
        private readonly IHomeRepository HomeRepository;

        public UsersService(IUserRespository userRepository, IHomeRepository homeRepository)
        {
            UserRepository = userRepository;
            HomeRepository = homeRepository;
        }

        public User Get(int userId, out string message)
        {
            try
            {
                if (userId == 0)
                {
                    message = "User id cannot be 0";
                    return null;
                }

                message = "";
                return UserRepository.Get(userId);
            }
            catch (Exception ex)
            {
                message = "Unknown error while getting user: " + ex;
                return null;
            }
        }

        public IEnumerable<User> GetAll(out string message)
        {
            try
            {
                message = "";
                return UserRepository.GetAll();
            }
            catch (Exception ex)
            {
                message = "Unknown error while getting all users: " + ex;
                return null;
            }
        }

        public User Login(string login, string password, out string message)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    message = "Login or Password is null or empty";
                    return null;
                }

                User user = UserRepository.GetUserByLogin(login);

                if (user != null)
                {
                    string passwordCrip = Criptography.GetSHA1(password);

                    if (login != user.Login || passwordCrip != user.Password)
                    {
                        message = "Invalid login or password";
                        return null;
                    }
                }
                else
                {
                    message = "User not found";
                    return null;
                }

                message = "";
                return user;
            }
            catch (Exception ex)
            {
                message = "Unknown error while loggin in user: " + ex;
                return null;
            }
        }

        public User Create(string login, string password, string nickname, int homeId, out string message)
        {
            try
            {
                Home home = HomeRepository.Get(homeId);

                if (home == null)
                {
                    message = "Home not found";
                    return null;
                }

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
                {
                    message = "Invalid parameters";
                    return null;
                }

                User user = new User
                {
                    Login = login,
                    Nickname = nickname,
                    Password = Criptography.GetSHA1(password),
                    Home = home
                };

                UserRepository.Save(user);

                message = "";
                return user;
            }
            catch (Exception ex)
            {
                message = "Unknown error while saving user to database: " + ex;
                return null;
            }
        }

        public User Create(string login, string password, string nickname, out string message)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
                {
                    message = "Invalid parameters";
                    return null;
                }

                User user = new User
                {
                    Login = login,
                    Nickname = nickname,
                    Password = Criptography.GetSHA1(password)
                };

                UserRepository.Save(user);

                message = "";
                return user;
            }
            catch (Exception ex)
            {
                message = "Unknown error while saving user to database: " + ex;
                return null;
            }
        }

        public bool Delete(int id, out string message)
        {
            try
            {
                if (UserRepository.CheckIfExists(id))
                {
                    UserRepository.Delete(id);

                    message = "";
                    return true;
                }
                else
                {
                    message = "User not found";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Unknown error while deleting user: " + ex;
                return false;
            }
        }

        public bool CheckIfExists(int id)
        {
            return UserRepository.CheckIfExists(id);
        }
    }
}
