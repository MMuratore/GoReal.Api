
namespace GoReal.Dal.Repository.Interfaces
{
    public interface IRoleRepository<TRole>
    {
        bool AddRoleToUser(string goTag, string roleName);
    }
}
