using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoTheDishesWebservice.ViewModels.UsersController
{
    public class CreateUserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public int? HomeId { get; set; }
    }
}
