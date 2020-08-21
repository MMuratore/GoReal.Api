using GoReal.Common.Interfaces.Enumerations;
using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IRoleRepository<TRole>
    {
        RoleResult CreateRole(string roleName);
        RoleResult AddRoleToUser(string goTag, string roleName);
        IEnumerable<TRole> GetUserRole(int userId);
    }
}
