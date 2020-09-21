using Tools.Databases;
using GoReal.Common.Interfaces;

using GoReal.Dal.Entities;
using GoReal.Dal.Repository;
using GoReal.Api.Models.Forms;

namespace GoReal.Api.Services
{
    public class RoleService : IRoleRepository<Role>
    {
        private readonly IRoleRepository<Role> _roleRepository;

        public RoleService(Connection connection)
        {
            _roleRepository = new RoleRepository(connection);

        }

        public bool AddRoleToUser(string goTag, string roleName)
        {
            return _roleRepository.AddRoleToUser(goTag, roleName);
        }

    }
}
