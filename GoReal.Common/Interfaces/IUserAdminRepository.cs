using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IUserAdminRepository<TEntity>
    {
        IEnumerable<TEntity> Get();
        bool Activate(int id);
        bool Ban(int id);
        bool Delete(int id);
    }
}
