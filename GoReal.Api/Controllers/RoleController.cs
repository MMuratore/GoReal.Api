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
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());

            }

            return Ok();
        }
    }
}
