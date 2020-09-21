using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IRepository<TEntity>
    {
        TEntity Get(int id);
        bool Create(TEntity entity);
        bool Update(int id, TEntity entity);
        bool Delete(int id);
    }
}
