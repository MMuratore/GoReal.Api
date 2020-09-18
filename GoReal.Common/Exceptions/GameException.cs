using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class GameException: Exception
    {
        public GameResult Result { get; set; }
        //public HttpStatusCode HttpStatusCode ‘get.set.

        public GameException() :base ("Game Exception") { }
        public GameException(string message) : base(message) { }
        public GameException(GameResult result, string message): base($"{(int)result} - {message}") 
        {
            Result = result;
        }
        //public ObjectResult createProblem
    }
}
