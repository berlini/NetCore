using DoTheDishesWebservice.DataAccess.Models;

namespace DoTheDishesWebservice.ViewModels.UsersController
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User UserLoggedIn { get; set; }
    }
}
