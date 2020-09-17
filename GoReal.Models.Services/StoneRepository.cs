using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System;
using System.Collections.Generic;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class StoneRepository : IStoneRepository<Stone>
    {
        private readonly Connection _connection;

        public StoneRepository(Connection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Stone> Get(int gameId)
        {
            Command cmd = new Command("GetStone", true);
            cmd.AddParameter("GameId", gameId);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToStone());
        }

        public GameResult AddStone(int gameId, Stone entity)
        {
            Command cmd = new Command("AddStone", true);
            cmd.AddParameter("GameId", gameId);
            cmd.AddParameter("Row", entity.Row);
            cmd.AddParameter("Column", entity.Column);
            cmd.AddParameter("Color", entity.Color);

            try
            {
                _connection.ExecuteNonQuery(cmd);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("PK_Stone")) return GameResult.PreventOverwrite;
            }
            return GameResult.ValidMove;
        }

        public bool DeleteStone(int gameId, Stone entity)
        {
            Command cmd = new Command("DeleteStone", true);
            cmd.AddParameter("GameId", gameId);
            cmd.AddParameter("Row", entity.Row);
            cmd.AddParameter("Column", entity.Column);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
