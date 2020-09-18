
namespace GoReal.Dal.Repository.Interfaces
{ 
    public interface IRepository<TEntity>
    {
        bool Create(TEntity entity);
        TEntity Get(int id);
        bool Update(int id, TEntity entity);
    }
}
