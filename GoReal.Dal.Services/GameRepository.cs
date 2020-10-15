using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Common.Interfaces;
using System.Linq;
using Tools.Databases;
using System.Collections.Generic;

namespace GoReal.Dal.Repository
{
    public class GameRepository : IRepository<Game>, IGameRepository<Game>
    {
        private readonly Connection _connection;

        public GameRepository(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Game> GetByUserId(int userId)
        {
            User user = new User();
            Command cmd = new Command("SELECT * FROM [Game] WHERE [BlackPlayerId] = @userId OR [WhitePlayerId] = @userId ORDER BY [StartDate] DESC");
            cmd.AddParameter("userId", userId);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToGame());
        }

        public Game Get(int id)
        {
            User user = new User();
            Command cmd = new Command("SELECT * FROM [Game] WHERE [GameId] = @Id");
            cmd.AddParameter("Id", id);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToGame()).SingleOrDefault();
        }

        public IEnumerable<Game> Get()
        {
            throw new System.NotImplementedException();
        }

        public Game Create(Game entity)
        {
            Command cmd = new Command("GameCreate", true);
            cmd.AddParameter("Size", entity.Size);
            cmd.AddParameter("Komi", entity.Komi);
            cmd.AddParameter("Handicap", entity.Handicap);
            cmd.AddParameter("TimeControlId", entity.TimeControlId);
            cmd.AddParameter("RuleId", entity.RuleId);
            cmd.AddParameter("BlackPlayerId", entity.BlackPlayerId);
            cmd.AddParameter("WhitePlayerId", entity.WhitePlayerId);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToGame()).SingleOrDefault();
        }

        public bool Update(int id, Game entity)
        {
            Command cmd = new Command("GameUpdate", true);
            cmd.AddParameter("GameId", id);
            cmd.AddParameter("Result", entity.Result);
            cmd.AddParameter("BlackCapture", entity.BlackCapture);
            cmd.AddParameter("WhiteCapture", entity.WhiteCapture);
            cmd.AddParameter("BlackState", entity.BlackState);
            cmd.AddParameter("WhiteState", entity.WhiteState);
            cmd.AddParameter("KoInfo", entity.KoInfo);

            return _connection.ExecuteNonQuery(cmd) <= 1;
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
