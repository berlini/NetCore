using DoTheDishesWebservice.Core.Utils;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DoTheDishesWebservice.Core.Services
{
    public class UsersService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly IHomeRepository HomeRepository;
        private readonly ILogger<UsersService> Logger;

        public UsersService(IUserRepository userRepository, IHomeRepository homeRepository, ILogger<UsersService> logger)
        {
            UserRepository = userRepository;
            HomeRepository = homeRepository;
            Logger = logger;
        }

        public User Get(int userId)
        {
            try
            {
                if (userId < 1)
                {
                    throw new ArgumentException("User id cannot lesser than 1");
                }

                return UserRepository.Get(userId);
            }
            catch (ArgumentException ex)
            {
                Logger.LogWarning("Error while getting user", ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while getting user", ex);
                throw new Exception("Unknown error while getting user");
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return UserRepository.GetAll();
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while getting all users", ex);
                throw new Exception("Unknown error while getting all users");
            }
        }

        public User Login(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Login or Password is null or empty");
                }

                User user = UserRepository.GetUserByLogin(login);

                if (user != null)
                {
                    string passwordCrip = Criptography.GetSHA1(password);

                    if (login != user.Login || passwordCrip != user.Password)
                    {
                        throw new ApplicationException("Invalid login or password");
                    }
                }
                else
                {
                    throw new ApplicationException("User not found");
                }

                return user;
            }
            catch (ArgumentException ex)
            {
                Logger.LogWarning("Error while loggin in user", ex);
                throw;
            }
            catch (ApplicationException ex)
            {
                Logger.LogWarning("Error while loggin in user", ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while loggin in user", ex);
                throw new Exception("Unknown error while loggin in user");
            }
        }

        public User Create(string login, string password, string nickname, int homeId)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
                {
                    throw new ArgumentException("Invalid parameters");
                }

                Home home = HomeRepository.Get(homeId);

                if (home == null)
                {
                    throw new ApplicationException("Home not found");
                }

                User user = new User
                {
                    Login = login,
                    Nickname = nickname,
                    Password = Criptography.GetSHA1(password),
                    Home = home
                };

                UserRepository.Save(user);

                return user;
            }
            catch (ArgumentException ex)
            {
                Logger.LogWarning("Error while saving user to database", ex);
                throw;
            }
            catch (ApplicationException ex)
            {
                Logger.LogWarning("Error while saving user to database", ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while saving user to database", ex);
                throw new Exception("Unknown error while saving user to database");
            }
        }

        public User Create(string login, string password, string nickname)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(nickname))
                {
                    throw new ArgumentException("Invalid parameters");
                }

                User user = new User
                {
                    Login = login,
                    Nickname = nickname,
                    Password = Criptography.GetSHA1(password)
                };

                UserRepository.Save(user);

                return user;
            }
            catch (ArgumentException ex)
            {
                Logger.LogWarning("Error while saving user to database", ex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while saving user to database", ex);
                throw new Exception("Unknown error while saving user to database");
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (CheckIfExists(id))
                {
                    UserRepository.Delete(id);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while deleting user", ex);
                throw new Exception("Unknown error while deleting user");
            }
        }

        public bool CheckIfExists(int id)
        {
            try
            {
                return UserRepository.CheckIfExists(id);
            }
            catch (Exception ex)
            {
                Logger.LogError("Unknown error while checking if user exists", ex);
                throw new Exception("Unknown error while checking if user exists");
            }
        }
    }
}
