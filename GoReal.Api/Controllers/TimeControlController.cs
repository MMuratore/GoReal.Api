using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using GoReal.Common.Exceptions;
using GoReal.Dal.Entities;
using GoReal.Api.Services;
using System.Collections.Generic;
using System.Linq;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class TimeControlController : ControllerBase
    {
        private readonly TimeControlService _timeControlService;

        public TimeControlController(TimeControlService TimeControlService)
        {
            _timeControlService = TimeControlService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ = new List<TimeControl>();
            List<TimeControl> timeControls;
            try
            {
                timeControls = _timeControlService.Get().ToList();
            }
            catch (CommonException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(timeControls);
        }
    }
}
