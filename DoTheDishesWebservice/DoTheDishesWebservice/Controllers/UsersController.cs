using System.Collections.Generic;
using System.Linq;
using DoTheDishesWebservice.Core.Services;
using DoTheDishesWebservice.DataAccess.Models;
using DoTheDishesWebservice.ViewModels.UsersController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DoTheDishesWebservice.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            IEnumerable<User> list = UserService.GetAll(out string message);

            if (list == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            if (list.ToList().Count == 0)
            {
                return NotFound();
            }

            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult<User> GetUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            User user = UserService.Get(id, out string message);

            if (user == null)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new LoginResponse
                {
                    Message = "Invalid parameters",
                    Success = false
                });
            }

            User user = UserService.Login(request.Login, request.Password, out string message);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new LoginResponse
                {
                    Message = message,
                    Success = false
                });
            }

            return Ok(new LoginResponse
            {
                Message = "",
                Success = true,
                UserLoggedIn = user
            });
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult CreateUser([FromBody] CreateUserRequest user)
        {
            if (user == null || string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Nickname))
            {
                return BadRequest();
            }

            User ret;
            string msg = "";

            if (user.HomeId == null || user.HomeId == 0)
            {
                ret = UserService.Create(user.Login, user.Password, user.Nickname, out string message);
                msg = message;
            }
            else
            {
                ret = UserService.Create(user.Login, user.Password, user.Nickname, (int)user.HomeId, out string message);
                msg = message;
            }

            if (ret != null)
            {
                return Ok(ret);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, msg);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (UserService.CheckIfExists(id))
            {
                bool success = UserService.Delete(id, out string message);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, message);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}