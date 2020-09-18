using System.Collections.Generic;

namespace GoReal.Dal.Repository.Interfaces
{
    public interface IUserRepository<TUser>
    {
        IEnumerable<TUser> Get();
        TUser Get(int userId);
        bool Update(int userId, TUser user);
        bool Delete(int userId);
        bool DeleteAdmin(int userId);
        bool Activate(int userId);
        bool Ban(int userId);
    }
}
