using System.Linq;
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

namespace GoReal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        IAuthRepository<D.User> _authService;
        IRoleRepository _roleService;
        ITokenService _tokenService;

        public AuthController(ITokenService TokenService, IAuthRepository<D.User> AuthService, IRoleRepository RoleService)
        {
            _authService = AuthService;
            _roleService = RoleService;
            _tokenService = TokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginForm form)
        {
            User user = _authService.Login(form.Email, form.Password)?.ToClient();
            if (user is null) 
                return Problem("Email or Password error", statusCode: (int)HttpStatusCode.NotFound);

            user.Roles = _roleService.GetUserRole(user.UserId).ToList();

            user.Token = _tokenService.EncodeToken(user, user => new Claim[] {  
                new Claim("UserId", user.UserId.ToString()),
                new Claim("GoTag", user.GoTag),
                new Claim("LastName", user.LastName), 
                new Claim("FirstName", user.FirstName), 
                new Claim("Email", user.Email)
            });

            return Ok(user);
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
                    return Problem("GoTag already used", statusCode: (int)HttpStatusCode.Forbidden);
                case UserResult.EmailNotUnique:
                    return Problem("Email already used", statusCode: (int)HttpStatusCode.Forbidden);
                default:
                    break;
            }
            return NotFound();
        }
    }
}
