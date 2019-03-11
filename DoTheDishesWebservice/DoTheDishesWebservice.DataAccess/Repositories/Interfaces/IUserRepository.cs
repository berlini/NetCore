using DoTheDishesWebservice.DataAccess.Models;

namespace DoTheDishesWebservice.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByLogin(string login);
    }
}
