using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IUserRepository<TEntity>
    {
        TEntity Get(int id);
        bool Update(int id, TEntity entity);
        bool UpdatePassword(int id, string password);
        bool Desactivate(int id);
    }
}
