using System.Net;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using Microsoft.AspNetCore.Mvc;
using GoReal.Models.Entities;
using GoReal.Models.Api.Forms;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired(Role.SuperAdministrator)]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository<Role> _roleService;

        public RoleController(IRoleRepository<Role> RoleService)
        {
            _roleService = RoleService;
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRoleForm form)
        {
            switch (_roleService.AddRoleToUser(form.GoTag, form.RoleName))
            {
                case RoleResult.Register:
                    return Ok();
                case RoleResult.UserRoleNotUnique:
                    return Problem("User already possess that role", statusCode: (int)HttpStatusCode.BadRequest);
                case RoleResult.UserNotExist:
                    return Problem("Unknown user", statusCode: (int)HttpStatusCode.BadRequest);
                case RoleResult.RoleNotExist:
                    return Problem("Unknown role", statusCode: (int)HttpStatusCode.BadRequest);
                default:
                    break;
            }
            return NotFound();
        }
    }
}
