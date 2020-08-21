using System.Net;
using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using Microsoft.AspNetCore.Mvc;
using GoReal.Models.Api.Mappers;
using GoReal.Models.Entities;
using GoReal.Models.Api.Forms;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Authorization;

namespace GoReal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired("Administrator")]
    public class RoleController : Controller
    {
        IRoleRepository<Role> _roleService;

        public RoleController(IRoleRepository<Role> RoleService)
        {
            _roleService = RoleService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Role role)
        {
            switch (_roleService.CreateRole(role.RoleName))
            {
                case RoleResult.Register:
                    return Ok();
                case RoleResult.RoleNameNotUnique:
                    return Problem("RoleName already used", statusCode: (int)HttpStatusCode.Forbidden);
                default:
                    break;
            }
            return NotFound();
        }

        [HttpPut]
        public IActionResult Put([FromBody] UserRoleForm form)
        {
            switch (_roleService.AddRoleToUser(form.GoTag, form.RoleName))
            {
                case RoleResult.Register:
                    return Ok();
                case RoleResult.UserRoleNotUnique:
                    return Problem("User already possess that role", statusCode: (int)HttpStatusCode.Forbidden);
                case RoleResult.UserNotExist:
                    return Problem("Unknown user", statusCode: (int)HttpStatusCode.Forbidden);
                case RoleResult.RoleNotExist:
                    return Problem("Unknown role", statusCode: (int)HttpStatusCode.Forbidden);
                default:
                    break;
            }
            return NotFound();
        }
    }
}
