using GoReal.Common.Interfaces.Enumerations;
using System;

namespace GoReal.Models.Api.Exceptions
{
    public class GameException: Exception
    {
        public GameResult GameResult { get; set; }

        public GameException() :base ("Game Exception") { }
        public GameException(string message) : base(message) { }
        public GameException(GameResult gameResult, string message): base($"{(int)gameResult} - {message}") 
        {
            GameResult = gameResult;
        }
    }
}
