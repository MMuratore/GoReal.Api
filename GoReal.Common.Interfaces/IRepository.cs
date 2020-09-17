using GoReal.Common.Interfaces.Enumerations;
using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IRepository<TEntity, TResult>
    {
        bool Create(TEntity entity);
        TEntity Get(int id);
        TResult Update(int id, TEntity entity);
    }
}
