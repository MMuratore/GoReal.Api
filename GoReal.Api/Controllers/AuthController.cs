using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GoReal.Common.Interfaces;
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
            {
                user.Token = _tokenService.EncodeToken(user, user => new Claim[] { 
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("GoTag", user.GoTag),
                    new Claim("LastName", user.LastName), 
                    new Claim("FirstName", user.FirstName), 
                    new Claim("Email", user.Email) });

                return Ok(user);
            }
            /*
            [HttpPost]
            [Route("loginAsync")]
            public async Task<IActionResult> loginAsync([FromBody] LoginForm form)
            {
                User user = await _authService.Login(form.Login, form.Password);

                if (!(user is null))
                {
                    user.Token = _tokenService.EncodeToken(user, user => new Claim[] { new Claim("Id", user.Id.ToString()), new Claim("LastName", user.LastName), new Claim("FirstName", user.FirstName), new Claim("Email", user.Email) });
                    user.Addresses = _addressService.GetByUserId(user.Id);
                }

                return Ok(user);
            }
            */

        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (_authService.Register(user)) return Ok();
            return NotFound();
        }
    }
}
