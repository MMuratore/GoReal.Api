using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IUserRepository<TEntity>
    {
        TEntity Get(int id);
        bool Update(int id, TEntity entity);
        bool Desactivate(int id);
    }
}
