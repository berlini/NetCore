using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoTheDishesWebservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepository<User> UserRepository;

        public UsersController(IRepository<User> userRepository)
        {
            UserRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            return UserRepository.GetAll();
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return UserRepository.Get(id);
        }
    }
}