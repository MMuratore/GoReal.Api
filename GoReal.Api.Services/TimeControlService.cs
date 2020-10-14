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
    public class TimeControlService : IRepository<TimeControl>
    {
        private readonly IRepository<TimeControl> _timeControlRepository;

        public TimeControlService(Connection connection)
        {
            _timeControlRepository = new TimeControlRepository(connection);
        }

        public TimeControl Create(TimeControl entity)
        {
            return _timeControlRepository.Create(entity);
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public TimeControl Get(int id)
        {
            return _timeControlRepository.Get(id);
        }

        public IEnumerable<TimeControl> Get()
        {
            _ = new List<TimeControl>();

            List<TimeControl> rules = _timeControlRepository.Get().ToList();

            if (rules.Count() == 0)
                throw new CommonException(CommonResult.NotFound, HttpStatusCode.NotFound, "Not Found");

            return rules;
        }

        public bool Update(int id, TimeControl entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
