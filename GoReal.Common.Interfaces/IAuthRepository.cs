using GoReal.Common.Interfaces.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces
{
    public interface IAuthRepository<TUser>
    {
        (TUser, UserResult) Login(string login, string password);
        UserResult Register(TUser user);
    }
}
