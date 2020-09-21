using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using GoReal.Common.Exceptions;
using GoReal.Common.Interfaces;
using GoReal.Api.Models;
using GoReal.Api.Services;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository<User> _userService;

        public UserController(UserService UserService)
        {
            _userService = UserService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = _userService.Get(id);
            if (user is null) return NotFound();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            try
            {
                if (!_userService.Update(id, user)) return NotFound();
            }
            catch (UserException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_userService.Desactivate(id)) return NotFound();

            return Ok();
        }
    }
}
