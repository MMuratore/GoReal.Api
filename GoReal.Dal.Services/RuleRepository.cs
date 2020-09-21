using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Common.Interfaces;
using System.Linq;
using Tools.Databases;
using System.Collections.Generic;

namespace GoReal.Dal.Repository
{
    public class RuleRepository : IRepository<Rule>
    {
        private readonly Connection _connection;

        public RuleRepository(Connection connection)
        {
            _connection = connection;
        }

        public Rule Get(int id)
        {
            User user = new User();
            Command cmd = new Command("SELECT * FROM [Rule] WHERE RuleId = @Id");
            cmd.AddParameter("Id", id);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToRule()).SingleOrDefault();
        }

        public bool Create(Rule entity)
        {
            Command cmd = new Command("RuleCreate", true);
            cmd.AddParameter("RuleName", entity.RuleName);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }

        public bool Update(int id, Rule entity)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Rule> Get()
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
