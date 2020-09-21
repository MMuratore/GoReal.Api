using GoReal.Api.Models;
using Tools.Databases;
using D = GoReal.Dal.Entities;
using GoReal.Common.Interfaces;
using GoReal.Dal.Repository;
using GoReal.Api.Services.Mappers;
using Tools.Security.Token;
using System.Security.Claims;

namespace GoReal.Api.Services
{
    public class AuthService : IAuthRepository<User>
    {
        private readonly IAuthRepository<D.User> _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(Connection connection, ITokenService TokenService)
        {
            _authRepository = new AuthRepository(connection);
            _tokenService = TokenService;
        }

        public User Login(string login, string password)
        {
            User user = new User();

            user = _authRepository.Login(login, password)?.ToClient();


            if (!(user is null))
            {
                user.Token = _tokenService.EncodeToken(user, data => new Claim[] {
                    new Claim("UserId", data.UserId.ToString()),
                    new Claim("GoTag", data.GoTag),
                    new Claim("LastName", data.LastName),
                    new Claim("FirstName", data.FirstName),
                    new Claim("Email", data.Email),
                    new Claim("Roles", ((int)data.Roles).ToString())
                });
            }

            return user;
        }

        public bool Register(User user)
        {
            return _authRepository.Register(user?.ToDal());
        }
    }
}
