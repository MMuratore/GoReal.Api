using GoReal.Api.Models;
using System.Collections.Generic;
using System.Linq;
using Tools.Databases;
using D = GoReal.Dal.Entities;
using GoReal.Common.Interfaces;
using GoReal.Dal.Repository;
using GoReal.Api.Services.Mappers;


namespace GoReal.Api.Services
{
    public class UserService : IUserRepository<User>, IUserAdminRepository<User>
    {
        private readonly UserRepository _userRepository;

        public UserService(Connection connection)
        {
            _userRepository = new UserRepository(connection);
        }

        public User Get(int id)
        {
            return _userRepository.Get(id)?.ToClient();
        }

        public bool Update(int id, User entity)
        {
            return _userRepository.Update(id, entity.ToDal());
        }

        public bool UpdatePassword(int id, string password)
        {
            return _userRepository.UpdatePassword(id, password);
        }

        public bool Desactivate(int id)
        {
            return _userRepository.Desactivate(id);
        }

        public IEnumerable<User> Get()
        {
            return _userRepository.Get().Select(x => x.ToClient());
        }

        public bool Activate(int id)
        {
            return _userRepository.Activate(id);
        }

        public bool Ban(int id)
        {
            return _userRepository.Ban(id);
        }

        public bool Delete(int id)
        {
            return _userRepository.Delete(id);
        }
    }
}
