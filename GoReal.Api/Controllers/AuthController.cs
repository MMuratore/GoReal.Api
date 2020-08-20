using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Results;
using GoReal.Api.Infrastrucutre;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Api;
using GoReal.Models.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tools.Security.Token;

namespace GoReal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        IAuthRepository<User> _authService;
        ITokenService _tokenService;

        public AuthController(ITokenService TokenService, IAuthRepository<User> AuthService)
        {
            _authService = AuthService;
            _tokenService = TokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] Login form)
        {
            User user = _authService.Login(form.Email, form.Password);
            if (user is null) return NotFound();

            user.Token = _tokenService.EncodeToken(user, user => new Claim[] { 
                new Claim("UserId", user.UserId.ToString()),
                new Claim("GoTag", user.GoTag),
                new Claim("LastName", user.LastName), 
                new Claim("FirstName", user.FirstName), 
                new Claim("Email", user.Email) });

            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            UserResult userResult = _authService.Register(user);
            if (userResult == UserResult.Register) return Ok();
            else if (userResult == UserResult.GoTagNotUnique) return Problem("GoTag already use", statusCode: (int)HttpStatusCode.Forbidden);
            else if (userResult == UserResult.EmailNotUnique) return Problem("Email already use", statusCode: (int)HttpStatusCode.Forbidden);

            return NotFound();
        }
    }
}
