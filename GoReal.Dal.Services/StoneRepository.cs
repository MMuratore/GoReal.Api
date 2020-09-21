using GoReal.Common.Exceptions;
using GoReal.Common.Exceptions.Enumerations;
using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using Tools.Databases;

namespace GoReal.Dal.Repository
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

        public bool AddStone(int gameId, Stone entity)
        {
            bool result = false;

            Command cmd = new Command("AddStone", true);
            cmd.AddParameter("GameId", gameId);
            cmd.AddParameter("Row", entity.Row);
            cmd.AddParameter("Column", entity.Column);
            cmd.AddParameter("Color", entity.Color);

            try
            {
                result = _connection.ExecuteNonQuery(cmd) == 1;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("PK_Stone"))
                    throw new GameException(GameResult.PreventOverwrite, HttpStatusCode.BadRequest, "Prevent Overwrite");
            }

            return result;
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
