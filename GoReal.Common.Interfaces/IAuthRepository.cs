using GoReal.Common.Interfaces.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces
{
    public interface IAuthRepository<TUser>
    {
        UserResult Register(TUser user);
        TUser Login(string login, string password);
        UserResult Update(int userId, TUser user);
        bool Delete(int userId);
    }
}
