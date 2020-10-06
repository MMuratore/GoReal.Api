using D = GoReal.Dal.Entities;
using Microsoft.AspNetCore.Mvc;
using GoReal.Api.Infrastrucutre;
using Microsoft.AspNetCore.Cors;
using GoReal.Api.Services;
using GoReal.Api.Models;
using GoReal.Common.Exceptions;
using System.Collections.Generic;

namespace GoReal.Api.Controllers
{
    [EnableCors("localhost")]
    [Route("api/[controller]")]
    [ApiController]
    [AuthRequired]
    public class StatisticController : ControllerBase
    {
        private readonly StatisticService _statisticService;

        public StatisticController(StatisticService StatisticService)
        {
            _statisticService = StatisticService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int userId)
        {
            _ = new Statistic();
            Statistic statistic;

            try
            {
                statistic = _statisticService.Get(userId);
            }
            catch (StatisticException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(statistic);
        }
    }
}
