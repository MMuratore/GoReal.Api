using GoReal.Common.Interfaces.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces
{
    public interface IStoneRepository<TStone>
    {
        IEnumerable<TStone> Get(int gameId);
        GameResult AddStone(int gameId, TStone entity);
        bool DeleteStone(int gameId, TStone entity);
    }
}
