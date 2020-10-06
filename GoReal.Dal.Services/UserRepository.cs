using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tools.Databases;

namespace GoReal.Dal.Repository
{
    public class UserRepository : IUserRepository<User>, IUserAdminRepository<User>
    {
        private readonly Connection _connection;

        public UserRepository(Connection connection)
        {
            _connection = connection;
        }

        public User Get(int id)
        {
            Command cmd = new Command("UserGet", true);
            cmd.AddParameter("UserId", id);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser()).SingleOrDefault();
        }

        public bool Update(int id, User user)
        {
            bool isUpdate = false;
            Command cmd = new Command("UserUpdate", true);
            cmd.AddParameter("UserId", id);
            cmd.AddParameter("GoTag", user.GoTag);
            cmd.AddParameter("LastName", user.LastName);
            cmd.AddParameter("FirstName", user.FirstName);
            cmd.AddParameter("Email", user.Email);

            try
            {
                isUpdate = _connection.ExecuteNonQuery(cmd) == 1;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UK_User_GoTag")) throw new UserException(UserResult.GoTagNotUnique, HttpStatusCode.BadRequest, "GoTag is already use");
                if (e.Message.Contains("UK_User_Email")) throw new UserException(UserResult.EmailNotUnique, HttpStatusCode.BadRequest, "Email is already use");
            }

            return isUpdate;
        }

        public bool UpdatePassword(int id, string password)
        {
            Command cmd = new Command("UserUpdatePassword", true);
            cmd.AddParameter("UserId", id);
            cmd.AddParameter("Password", password);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public bool Desactivate(int id)
        {
            Command cmd = new Command("UserDelete", true);
            cmd.AddParameter("userId", id);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public IEnumerable<User> Get()
        {
            Command cmd = new Command("UserGetAll", true);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser());
        }

        public bool Delete(int id)
        {
            Command cmd = new Command("UserDeleteAdmin", true);
            cmd.AddParameter("userId", id);

            return _connection.ExecuteNonQuery(cmd) >= 1;
        }

        public bool Activate(int id)
        {
            Command cmd = new Command("UserActivate", true);
            cmd.AddParameter("userId", id);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public bool Ban(int id)
        {
            Command cmd = new Command("UserBan", true);
            cmd.AddParameter("userId", id);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
