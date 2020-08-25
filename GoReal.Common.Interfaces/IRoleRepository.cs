using GoReal.Common.Interfaces.Enumerations;
using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IRoleRepository<TRole>
    {
        RoleResult AddRoleToUser(string goTag, string roleName);
        TRole GetUserRole(int userId);
    }
}
