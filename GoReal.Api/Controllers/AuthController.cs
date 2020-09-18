using System.Net;
using System.Security.Claims;
using D = GoReal.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;
using Microsoft.AspNetCore.Cors;
using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Repository.Interfaces;
using GoReal.Api.Models.Forms;
using GoReal.Api.Models;
using GoReal.Api.Services.Mappers;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository<D.User> _authService;
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService TokenService, IAuthRepository<D.User> AuthService)
        {
            _authService = AuthService;
            _tokenService = TokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            User user = new User();

            try
            {
                user = _authService.Login(form.Email, form.Password)?.ToClient();
            }
            catch (UserException exception)
            {
                return exception.Result switch
                {
                    UserResult.Ban => Problem(((int)UserResult.Ban).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    UserResult.Inactive => Problem(((int)UserResult.Inactive).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => NotFound(),
                };
            }

            if (user is null) return NotFound();

            user.Token = _tokenService.EncodeToken(user, user => new Claim[] {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("GoTag", user.GoTag),
                new Claim("LastName", user.LastName),
                new Claim("FirstName", user.FirstName),
                new Claim("Email", user.Email),
                new Claim("Roles", ((int)user.Roles).ToString())
            });

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                if (!_authService.Register(user?.ToDal())) return NotFound();
            }
            catch (UserException exception)
            {
                return exception.Result switch
                {
                    UserResult.GoTagNotUnique => Problem(((int)UserResult.GoTagNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    UserResult.EmailNotUnique => Problem(((int)UserResult.EmailNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => NotFound(),
                };
            }

            return Ok();
        }
    }
}
