using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Api.Models;
using System.Collections.Generic;
using System.Linq;
using Tools.Databases;
using D = GoReal.Dal.Entities;
using GoReal.Common.Interfaces;
using GoReal.Dal.Repository;
using GoReal.Api.Services.Mappers;
using System.Net;

namespace GoReal.Api.Services
{
    public class StatisticService
    {
        private readonly StatisticRepository _statisticRepository;

        public StatisticService(Connection connection)
        {
            _statisticRepository = new StatisticRepository(connection);
        }

        public Statistic Get(int userId)
        {
            _ = new Statistic();

            Statistic statistic = _statisticRepository.Get(userId).ToClient();

            if (statistic is null)
                throw new CommonException(CommonResult.NotFound, HttpStatusCode.NotFound, "User do not exist");

            return statistic;
        }
    }
}
