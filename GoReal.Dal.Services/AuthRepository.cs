using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Dal.Repository.Interfaces;
using System;
using System.Data.SqlClient;
using System.Linq;
using Tools.Databases;

namespace GoReal.Dal.Repository
{
    public class AuthRepository : IAuthRepository<User>
    {
        private readonly Connection _connection;

        public AuthRepository(Connection connection)
        {
            _connection = connection;
        }

        public User Login(string login, string password)
        {
            User user = new User();
            Command cmd = new Command("Login", true);
            cmd.AddParameter("Email", login);
            cmd.AddParameter("Password", password);
            try
            {
                user = _connection.ExecuteReader(cmd, (dr) => dr.ToUser()).SingleOrDefault();
            }
            catch (SqlException e)
            {
                if (e.State == 4) throw new UserException(UserResult.Ban, "user ban");
                if (e.State == 5) throw new UserException(UserResult.Inactive, "user inactive");
            }

            return user;
        }

        public bool Register(User user)
        {
            bool isRegister = false;
            Command cmd = new Command("Register", true);
            cmd.AddParameter("GoTag", user.GoTag);
            cmd.AddParameter("LastName", user.LastName);
            cmd.AddParameter("FirstName", user.FirstName);
            cmd.AddParameter("Email", user.Email);
            cmd.AddParameter("Password", user.Password);
            try
            {
                isRegister = _connection.ExecuteNonQuery(cmd) == 1;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("UK_User_GoTag")) throw new UserException(UserResult.GoTagNotUnique, "GoTag is already use");
                if (e.Message.Contains("UK_User_Email")) throw new UserException(UserResult.EmailNotUnique, "Email is already use");
            }
            return isRegister;
        }
    }
}
