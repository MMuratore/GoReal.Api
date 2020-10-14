using Tools.Databases;
using GoReal.Common.Interfaces;

using GoReal.Dal.Entities;
using GoReal.Dal.Repository;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Common.Exceptions;

namespace GoReal.Api.Services
{
    public class RuleService : IRepository<Rule>
    {
        private readonly IRepository<Rule> _ruleRepository;

        public RuleService(Connection connection)
        {
            _ruleRepository = new RuleRepository(connection);
        }

        public Rule Create(Rule entity)
        {
            return _ruleRepository.Create(entity);  
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Rule Get(int id)
        {
            return _ruleRepository.Get(id);
        }

        public IEnumerable<Rule> Get()
        {
            _ = new List<Rule>();

            List<Rule> rules = _ruleRepository.Get().ToList();

            if (rules.Count() == 0)
                throw new CommonException(CommonResult.NotFound, HttpStatusCode.NotFound, "Not Found");

            return rules;
        }

        public bool Update(int id, Rule entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
