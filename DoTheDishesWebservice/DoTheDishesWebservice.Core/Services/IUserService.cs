using DoTheDishesWebservice.DataAccess.Models;
using System.Collections.Generic;

namespace DoTheDishesWebservice.Core.Services
{
    public interface IUserService
    {
        User Login(string login, string password);
        User Get(int userId);
        IEnumerable<User> GetAll();
        User Create(string login, string password, string nickname, int homeId);
        User Create(string login, string password, string nickname);
        void Delete(int id);
        bool CheckIfExists(int id);
    }
}
