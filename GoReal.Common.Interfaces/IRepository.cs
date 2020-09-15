using GoReal.Common.Interfaces.Enumerations;
using System.Collections.Generic;

namespace GoReal.Common.Interfaces
{
    public interface IRepository<Tentity>
    {
        bool Create(Tentity entity);
        Tentity Get(int id);
    }
}
