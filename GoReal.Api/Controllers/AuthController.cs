using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;
using Microsoft.AspNetCore.Cors;
using GoReal.Common.Exceptions;
using GoReal.Api.Models.Forms;
using GoReal.Api.Models;
using GoReal.Api.Services;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService AuthService, ITokenService TokenService)
        {
            _authService = AuthService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            _ = new User();
            User user;
            try
            {
                user = _authService.Login(form.Email, form.Password);
            }
            catch (UserException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                if (!_authService.Register(user)) return NotFound();
            }
            catch (UserException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }

            return Ok();
        }
    }
}
