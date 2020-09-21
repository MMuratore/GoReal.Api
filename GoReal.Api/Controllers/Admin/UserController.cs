using D = GoReal.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using GoReal.Api.Models;
using GoReal.Api.Services;
using GoReal.Common.Interfaces;

namespace GoReal.Api.Controllers.Admin
{
    [EnableCors("localhost")]
    [Route("api/admin/[controller]")]
    [ApiController]
    [AuthRequired(D.Role.SuperAdministrator)]
    public class UserController : ControllerBase
    {
        private readonly IUserAdminRepository<User> _userService;

        public UserController(UserService UserService)
        {
            _userService = UserService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userService.Get());
        }

        [HttpPatch]
        [Route("activate/{id}")]
        public IActionResult Activate(int id)
        {
            if (_userService.Activate(id)) return Ok();

            return NotFound();
        }

        [HttpPatch]
        [Route("ban/{id}")]
        public IActionResult Ban(int id)
        {
            if (_userService.Ban(id)) return Ok();

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_userService.Delete(id)) return NotFound();

            return Ok();
        }
    }
}
