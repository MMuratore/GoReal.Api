using System;
using System.Collections.Generic;
using System.Text;

namespace GoReal.Common.Interfaces
{
    public interface IAuthRepository<TUser>
    {
        bool Register(TUser user);
        TUser Login(string login, string password);
    }
}
