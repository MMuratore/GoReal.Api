using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System.Linq;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class RuleRepository : IRepository<Rule, RuleResult>
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

        public RuleResult Update(int id, Rule entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
