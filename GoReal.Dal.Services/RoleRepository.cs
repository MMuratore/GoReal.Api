using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Interfaces;
using System;
using Tools.Databases;

namespace GoReal.Dal.Repository
{
    public class RoleRepository : IRoleRepository<Role>
    {
        private readonly Connection _connection;

        public RoleRepository(Connection connection)
        {
            _connection = connection;
        }

        public bool AddRoleToUser(string goTag, string roleName)
        {
            bool result = false;
            Command cmd = new Command("AddRoleToUser", true);
            cmd.AddParameter("GoTag", goTag);
            cmd.AddParameter("RoleName", roleName);

            try
            {
                result = _connection.ExecuteNonQuery(cmd) == 1;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("PK_UserRole")) throw new RoleException(RoleResult.UserRoleNotUnique, "User has already this role");
                if (e.Message.Contains("NULL into column 'UserId'")) throw new RoleException(RoleResult.UserNotExist, "User not exist");
                if (e.Message.Contains("NULL into column 'RoleId'")) throw new RoleException(RoleResult.RoleNotExist, "Role not exist");
            }
            return result;
        }
    }
}
