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
    public class RuleController : ControllerBase
    {
        private readonly RuleService _ruleService;

        public RuleController(RuleService RuleService)
        {
            _ruleService = RuleService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ = new List<Rule>();
            List<Rule> rules;
            try
            {
                rules = _ruleService.Get().ToList();
            }
            catch (CommonException exception)
            {
                return Problem(exception.Result.ToString(), statusCode: (int)exception.HttpStatusCode, type: ((int)exception.Result).ToString());
            }
            return Ok(rules);
        }
    }
}
