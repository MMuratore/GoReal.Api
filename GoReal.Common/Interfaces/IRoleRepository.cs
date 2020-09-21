
namespace GoReal.Common.Interfaces
{
    public interface IRoleRepository<TRole>
    {
        bool AddRoleToUser(string goTag, string roleName);
    }
}
