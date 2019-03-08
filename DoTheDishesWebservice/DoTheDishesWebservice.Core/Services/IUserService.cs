using DoTheDishesWebservice.DataAccess.Models;
using System.Collections.Generic;

namespace DoTheDishesWebservice.Core.Services
{
    public interface IUserService
    {
        User Login(string login, string password, out string message);
        User Get(int userId, out string message);
        IEnumerable<User> GetAll(out string message);
        User Create(string login, string password, string nickname, int homeId, out string message);
        User Create(string login, string password, string nickname, out string message);
        bool Delete(int id, out string message);
        bool CheckIfExists(int id);
    }
}
