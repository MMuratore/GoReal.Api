using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IGameRepository<TGame>
    {
        IEnumerable<TGame> GetByUserId(int userId);
    }
}
