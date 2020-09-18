using System.Net;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Interfaces;
using GoReal.Api.Models.Forms;

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
            try
            {
                if(!_roleService.AddRoleToUser(form.GoTag, form.RoleName)) return NotFound();
            }
            catch (RoleException exception)
            {
                return exception.Result switch
                {
                    RoleResult.UserRoleNotUnique => Problem(((int)RoleResult.UserRoleNotUnique).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    RoleResult.UserNotExist => Problem(((int)RoleResult.UserNotExist).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    RoleResult.RoleNotExist => Problem(((int)RoleResult.RoleNotExist).ToString(), statusCode: (int)HttpStatusCode.BadRequest),
                    _ => NotFound(),
                };
            }

            return Ok();
        }
    }
}
