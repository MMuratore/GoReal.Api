using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Dal.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Databases;

namespace GoReal.Dal.Repository
{
    public class UserRepository : IUserRepository<User>
    {
        private readonly Connection _connection;

        public UserRepository(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<User> Get()
        {
            Command cmd = new Command("UserGetAll", true);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser());
        }

        public User Get(int userId)
        {
            Command cmd = new Command("UserGet", true);
            cmd.AddParameter("UserId", userId);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser()).SingleOrDefault();
        }

        public bool Update(int userId, User user)
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
                if (e.Message.Contains("UK_User_GoTag")) throw new UserException(UserResult.GoTagNotUnique, "GoTag is already use");
                if (e.Message.Contains("UK_User_Email")) throw new UserException(UserResult.EmailNotUnique, "Email is already use");
            }

            return isUpdate;
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
