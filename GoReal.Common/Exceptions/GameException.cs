using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class GameException: Exception
    {
        public GameResult Result { get; }
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;

        public GameException() :base ("Game Exception") { }
        public GameException(string message) : base(message) { }
        public GameException(GameResult result, string message): base($"{(int)result} - {message}") 
        {
            Result = result;
        }
        public GameException(GameResult result, HttpStatusCode httpStatusCode, string message) : this(result, $"{(int)result} - {message}")
        {
            HttpStatusCode = httpStatusCode;
        }

    }
}
