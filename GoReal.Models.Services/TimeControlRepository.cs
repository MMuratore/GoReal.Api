using GoReal.Common.Interfaces;
using GoReal.Common.Interfaces.Enumerations;
using GoReal.Models.Entities;
using GoReal.Models.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Tools.Databases;

namespace GoReal.Models.Services
{
    public class TimeControlRepository : IRepository<TimeControl>
    {
        private readonly Connection _connection;

        public TimeControlRepository(Connection connection)
        {
            _connection = connection;
        }

        public TimeControl Get(int id)
        {
            User user = new User();
            Command cmd = new Command("SELECT * FROM [TimeControl] WHERE TimeControlId = @Id");
            cmd.AddParameter("Id", id);

            return _connection.ExecuteReader(cmd, (dr) => dr.ToTimeControl()).SingleOrDefault();
        }

        public bool Create(TimeControl entity)
        {
            Command cmd = new Command("GameCreate", true);
            cmd.AddParameter("Speed", entity.Speed);
            cmd.AddParameter("OverTime", entity.OverTime);
            cmd.AddParameter("TimeLimit", entity.TimeLimit);
            cmd.AddParameter("TimePerPeriod", entity.TimePerPeriod);
            cmd.AddParameter("Period", entity.Period);
            cmd.AddParameter("InitialTime", entity.InitialTime);

            return _connection.ExecuteNonQuery(cmd) == 1;
        }
    }
}
