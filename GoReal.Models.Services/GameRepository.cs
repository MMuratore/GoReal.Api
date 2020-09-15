using GoReal.Common.Interfaces;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System.Linq;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class GameRepository : IRepository<Game>
    {
        private readonly Connection _connection;

        public GameRepository(Connection connection)
        {
            _connection = connection;
        }

        public Game Get(int id)
        {
            User user = new User();
            Command cmd = new Command("SELECT * FROM [Game] WHERE [GameId] = @Id");
            cmd.AddParameter("Id", id);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToGame()).SingleOrDefault();
        }

        public bool Create(Game game)
        {
            Command cmd = new Command("GameCreate", true);
            cmd.AddParameter("Date", game.Date);
            cmd.AddParameter("Size", game.Size);
            cmd.AddParameter("Komi", game.Komi);
            cmd.AddParameter("Handicap", game.Handicap);
            cmd.AddParameter("TimeControlId", game.TimeControlId);
            cmd.AddParameter("RuleId", game.RuleId);
            cmd.AddParameter("BlackPlayerId", game.BlackPlayerId);
            cmd.AddParameter("WhitePlayerId", game.WhitePlayerId);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
