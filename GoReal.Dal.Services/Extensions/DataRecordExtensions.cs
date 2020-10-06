using GoReal.Dal.Entities;
using System;
using System.Data;

namespace GoReal.Dal.Repository.Extensions
{
    public static class DataRecordExtensions
    {
        internal static User ToUser(this IDataRecord Dr)
        {
            return new User() {
                UserId = (int)Dr["UserId"],
                GoTag = (string)Dr["GoTag"],
                LastName = (string)Dr["LastName"],
                FirstName = (string)Dr["FirstName"],
                Email = (string)Dr["Email"],
                isActive = (bool)Dr["isActive"],
                isBan = (bool)Dr["isBan"],
                Roles = (int)Dr["Role"]
            };
        }

        internal static Stone ToStone(this IDataRecord Dr)
        {
            return new Stone()
            {
                Row = (int)Dr["Row"],
                Column = (int)Dr["Column"],
                Color = (bool)Dr["Color"]
            };
        }

        internal static Game ToGame(this IDataRecord Dr)
        {
            return new Game()
            {
                Id = (int)Dr["GameId"],
                StartDate = (DateTime)Dr["StartDate"],
                EndDate = Dr["EndDate"] != DBNull.Value ? (DateTime?)Dr["EndDate"] : null,
                BlackRank = (int)Dr["BlackRank"],
                WhiteRank = (int)Dr["WhiteRank"],
                Result = Dr["Result"] != DBNull.Value ? (string)Dr["Result"] : null,
                Size = (int)Dr["Size"],
                Komi = (int)Dr["Komi"],
                Handicap = (int)Dr["Handicap"],
                BlackCapture = (int)Dr["BlackCapture"],
                WhiteCapture = (int)Dr["WhiteCapture"],
                BlackState = Dr["BlackState"] != DBNull.Value ? (bool?)Dr["BlackState"] : null,
                WhiteState = Dr["WhiteState"] != DBNull.Value ? (bool?)Dr["WhiteState"] : null,
                KoInfo = Dr["KoInfo"] != DBNull.Value ? (string)Dr["KoInfo"] : null,
                TimeControlId = (int)Dr["TimeControlId"],
                RuleId = (int)Dr["RuleId"],
                BlackPlayerId = (int)Dr["BlackPlayerId"],
                WhitePlayerId = (int)Dr["WhitePlayerId"]
            };
        }

        internal static Entities.Rule ToRule(this IDataRecord Dr)
        {
            return new Entities.Rule()
            {
                Id = (int)Dr["RuleId"],
                RuleName = (string)Dr["RuleName"],
                Overwrite = (bool)Dr["Overwrite"],
                Suicide = (bool)Dr["Suicide"],
                Ko = (bool)Dr["Ko"]
            };
        }

        internal static TimeControl ToTimeControl(this IDataRecord Dr)
        {
            return new TimeControl()
            {
                Id = (int)Dr["TimeControlId"],
                Speed = (string)Dr["Speed"],
                OverTime = (string)Dr["OverTime"],
                TimeLimit = Dr["TimeLimit"] != DBNull.Value ? (int?)Dr["TimeLimit"] : null,
                TimePerPeriod = Dr["TimePerPeriod"] != DBNull.Value ? (int?)Dr["TimePerPeriod"] : null,
                Period = Dr["Period"] != DBNull.Value ? (int?)Dr["Period"] : null,
                InitialTime = Dr["InitialTime"] != DBNull.Value ? (int?)Dr["InitialTime"] : null
            };
        }

        internal static Statistic ToStatistic(this IDataRecord Dr)
        {
            return new Statistic()
            {
                UserId = (int)Dr["UserId"],
                GameNumber = (int)Dr["GameNumber"],
                VictoryRatio = (decimal)Dr["VictoryRatio"],
                PlayTime = (int)Dr["PlayTime"]
            };
        }
    }
}
