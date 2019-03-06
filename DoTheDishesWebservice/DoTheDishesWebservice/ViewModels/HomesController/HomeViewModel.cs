using DoTheDishesWebservice.ViewModels.UsersController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoTheDishesWebservice.ViewModels.HomesController
{
    public class HomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<UserViewModel> Users { get; set; }
    }
}
