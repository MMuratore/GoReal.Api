using GoReal.Common.Interfaces.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces
{
    public interface IUserRepository<TUser>
    {
        IEnumerable<TUser> Get();
        TUser Get(int userId);
        UserResult Update(int userId, TUser user);
        bool Delete(int userId);
        bool DeleteAdmin(int userId);
        bool Activate(int userId);
        bool Ban(int userId);
    }
}
