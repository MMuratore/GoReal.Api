using System.Net;
using System.Security.Claims;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Api;
using D = GoReal.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;
using GoReal.Models.Api.Mappers;
using GoReal.Models.Api.Forms;
using Microsoft.AspNetCore.Cors;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthRepository<D.User> _authService;
        ITokenService _tokenService;

        public AuthController(ITokenService TokenService, IAuthRepository<D.User> AuthService)
        {
            _authService = AuthService;
            _tokenService = TokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            var results = _authService.Login(form.Email, form.Password);
            User user = results.Item1?.ToClient();

            switch (results.Item2)
            {
                case UserResult.Login:
                    user.Token = _tokenService.EncodeToken(user, user => new Claim[] {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("GoTag", user.GoTag),
                        new Claim("LastName", user.LastName),
                        new Claim("FirstName", user.FirstName),
                        new Claim("Email", user.Email),
                        new Claim("Roles", ((int)user.Roles).ToString())
                    });
                    return Ok(user);
                case UserResult.Ban:
                    return Problem(((int)UserResult.Ban).ToString(), statusCode: (int)HttpStatusCode.NotFound);
                case UserResult.Inactive:
                    return Problem(((int)UserResult.Inactive).ToString(), statusCode: (int)HttpStatusCode.NotFound);
                default:
                    break;
            }
            
            return NotFound();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            switch (_authService.Register(user.ToDal()))
            {
                case UserResult.Register:
                    return Ok();
                case UserResult.GoTagNotUnique:
                    return Problem(((int)UserResult.GoTagNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest);
                case UserResult.EmailNotUnique:
                    return Problem(((int)UserResult.EmailNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest);
                default:
                    break;
            }
            return NotFound();
        }
    }
}
