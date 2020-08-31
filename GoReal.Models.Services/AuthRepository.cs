using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class AuthRepository : IAuthRepository<User>
    {
        private Connection _connection;

        public AuthRepository(Connection connection)
        {
            _connection = connection;
        }

        public (User, UserResult) Login(string login, string password)
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
                if (e.State == 4) return (user, UserResult.Ban);
                if (e.State == 5) return (user, UserResult.Inactive);
            }
            if(user is null)
                return (user, UserResult.Failed);
            else
                return (user, UserResult.Login);
        }

        public UserResult Register(User user)
        {
            Command cmd = new Command("Register", true);
            cmd.AddParameter("GoTag", user.GoTag);
            cmd.AddParameter("LastName", user.LastName);
            cmd.AddParameter("FirstName", user.FirstName);
            cmd.AddParameter("Email", user.Email);
            cmd.AddParameter("Password", user.Password);
            try
            {
                _connection.ExecuteNonQuery(cmd);
            }
            catch (Exception e)
            {
                if(e.Message.Contains("UK_User_GoTag")) return UserResult.GoTagNotUnique;
                if(e.Message.Contains("UK_User_Email")) return UserResult.EmailNotUnique;
            }
            return UserResult.Register;
        }
    }
}
