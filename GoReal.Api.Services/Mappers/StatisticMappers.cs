using GoReal.Api.Models;
using D = GoReal.Dal.Entities;

namespace GoReal.Api.Services.Mappers
{ 
    public static class StatisticMappers
    {
        public static Statistic ToClient(this D.Statistic entity)
        {
            return new Statistic() { 
                UserId = entity.UserId,
                GameNumber = entity.GameNumber,
                VictoryRatio = entity.VictoryRatio,
                PlayTime = entity.PlayTime
            };
        }
    }
}
