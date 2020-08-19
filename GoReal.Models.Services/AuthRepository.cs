using GoReal.Common.Interfaces;
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

        public AuthRepository()
        {
            _connection = new Connection(new ConnectionInfo("Data Source = MURAKS; Initial Catalog = Library; Integrated Security = True;"), SqlClientFactory.Instance);
        }

        public User Login(string login, string password)
        {
            Command cmd = new Command("Login", true);
            cmd.AddParameter("Email", login);
            cmd.AddParameter("Password", password);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToUser()).SingleOrDefault();
        }

        public bool Register(User user)
        {
            Command cmd = new Command("Register", true);
            cmd.AddParameter("GoTag", user.GoTag);
            cmd.AddParameter("LastName", user.LastName);
            cmd.AddParameter("FirstName", user.FirstName);
            cmd.AddParameter("Email", user.Email);
            cmd.AddParameter("Password", user.Password);
            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
