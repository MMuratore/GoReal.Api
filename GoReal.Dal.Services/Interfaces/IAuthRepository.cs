
namespace GoReal.Dal.Repository.Interfaces
{
    public interface IAuthRepository<TUser>
    {
        TUser Login(string login, string password);
        bool Register(TUser user);
    }
}
