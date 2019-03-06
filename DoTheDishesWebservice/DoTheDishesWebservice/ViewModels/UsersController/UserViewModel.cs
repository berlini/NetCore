using DoTheDishesWebservice.ViewModels.HomesController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoTheDishesWebservice.ViewModels.UsersController
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }

        public HomeViewModel Home { get; set; }
    }
}
