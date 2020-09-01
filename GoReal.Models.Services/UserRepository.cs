using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System;
using System.Collections.Generic;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class UserRepository : IUserRepository<User>
    {
        private Connection _connection;

        public UserRepository(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<User> Get()
        {
            Command cmd = new Command("UserGetAll", true);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser());
        }

        public UserResult Update(int userId, User user)
        {
            bool isUpdate = false;
            Command cmd = new Command("UserUpdate", true);
            cmd.AddParameter("UserId", userId);
            cmd.AddParameter("GoTag", user.GoTag);
            cmd.AddParameter("LastName", user.LastName);
            cmd.AddParameter("FirstName", user.FirstName);
            cmd.AddParameter("Email", user.Email);
            cmd.AddParameter("Password", user.Password);
            try
            {
                isUpdate = _connection.ExecuteNonQuery(cmd) == 1;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UK_User_GoTag")) return UserResult.GoTagNotUnique;
                if (e.Message.Contains("UK_User_Email")) return UserResult.EmailNotUnique;
            }
            if(isUpdate)
                return UserResult.Update;
            else
                return UserResult.Failed;
        }

        public bool Activate(int userId)
        {
            Command cmd = new Command("UserActivate", true);
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public bool Delete(int userId)
        {
            Command cmd = new Command("UserDelete", true);
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public bool DeleteAdmin(int userId)
        {
            Command cmd = new Command("UserDeleteAdmin", true);
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteNonQuery(cmd) >= 1;
        }

        public bool Ban(int userId)
        {
            Command cmd = new Command("UserBan", true);
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
