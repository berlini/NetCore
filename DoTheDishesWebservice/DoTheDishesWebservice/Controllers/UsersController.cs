using System;
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
            try
            {
                IEnumerable<User> list = UserService.GetAll();

                if (list == null || list.ToList().Count == 0)
                {
                    return NotFound();
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult<User> GetUser(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest();
                }

                User user = UserService.Get(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new LoginResponse
                    {
                        Message = "Invalid parameters",
                        Success = false
                    });
                }

                User user = UserService.Login(request.Login, request.Password);

                return Ok(new LoginResponse
                {
                    Message = "",
                    Success = true,
                    UserLoggedIn = user
                });
            }
            catch (ApplicationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new LoginResponse
                {
                    Message = ex.Message,
                    Success = false
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new LoginResponse
                {
                    Message = ex.Message,
                    Success = false
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public ActionResult CreateUser([FromBody] CreateUserRequest user)
        {
            try
            {
                if (user == null || string.IsNullOrEmpty(user.Login) || string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Nickname))
                {
                    return BadRequest();
                }

                User ret;

                if (user.HomeId == null || user.HomeId == 0)
                {
                    ret = UserService.Create(user.Login, user.Password, user.Nickname);
                }
                else
                {
                    ret = UserService.Create(user.Login, user.Password, user.Nickname, (int)user.HomeId);
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                if (UserService.CheckIfExists(id))
                {
                    UserService.Delete(id);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}