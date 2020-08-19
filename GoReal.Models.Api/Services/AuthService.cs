using GoReal.Common.Interfaces;
using D = GoReal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using GoReal.Models.Services;
using GoReal.Models.Api.Mappers;

namespace GoReal.Models.Api.Services
{
    public class AuthService : IAuthRepository<User>
    {
        IAuthRepository<D.User> _globalRepository;

        public AuthService(string connectionString)
        {
            _globalRepository = new AuthRepository(connectionString);
        }

        public User Login(string login, string password)
        {
            return _globalRepository.Login(login, password)?.ToClient();
        }

        public bool Register(User user)
        {
            return _globalRepository.Register(user.ToDal());
        }

    }
}
