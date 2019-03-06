using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoTheDishesWebservice.DataAccess.Models
{
    public class Home
    {
        public int HomeId { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
