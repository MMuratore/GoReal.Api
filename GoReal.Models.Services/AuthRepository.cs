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
    public class AuthRepository : IAuthRepository<User>
    {
        private Connection _connection;

        public AuthRepository(string connectionString)
        {
            _connection = new Connection(new ConnectionInfo(connectionString), SqlClientFactory.Instance);
        }

        public User Login(string login, string password)
        {
            Command cmd = new Command("Login", true);
            cmd.AddParameter("Email", login);
            cmd.AddParameter("Password", password);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser()).SingleOrDefault();
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

        public UserResult Update(int userId, User user)
        {
            bool isUpdate = false;
            Command cmd = new Command("UpdateUser", true);
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

        public bool Delete(int userId)
        {
            Command cmd = new Command("DeleteUser", true);
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
