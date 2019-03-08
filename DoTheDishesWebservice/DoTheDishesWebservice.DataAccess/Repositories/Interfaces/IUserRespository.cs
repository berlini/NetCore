using DoTheDishesWebservice.DataAccess.Models;

namespace DoTheDishesWebservice.DataAccess.Repositories.Interfaces
{
    public interface IUserRespository : IRepository<User>
    {
        User GetUserByLogin(string login);
    }
}
