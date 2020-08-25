using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class RoleRepository : IRoleRepository<Role>
    {
        private Connection _connection;

        public RoleRepository(string connectionString)
        {
            _connection = new Connection(new ConnectionInfo(connectionString), SqlClientFactory.Instance);
        }

        public RoleResult AddRoleToUser(string goTag, string roleName)
        {
            Command cmd = new Command("AddRoleToUser", true);
            cmd.AddParameter("GoTag", goTag);
            cmd.AddParameter("RoleName", roleName);
            try
            {
                _connection.ExecuteNonQuery(cmd);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("PK_UserRole")) return RoleResult.UserRoleNotUnique;
                if (e.Message.Contains("NULL into column 'UserId'")) return RoleResult.UserNotExist;
                if (e.Message.Contains("NULL into column 'RoleId'")) return RoleResult.RoleNotExist;
            }
            return RoleResult.Register;
        }

        public Role GetUserRole(int userId)
        {
            Role userRole = Role.None;

            Command cmd = new Command("GetUserRole", true);
            cmd.AddParameter("UserId", userId);
            IEnumerable<Role> roles = _connection.ExecuteReader(cmd, (dr) => dr.ToRole());
            foreach (Role role in roles) { userRole |= role; }

            return userRole;
        }
    }
}
