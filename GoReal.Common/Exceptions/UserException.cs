using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class UserException : Exception
    {
        public UserResult Result { get; }
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;

        public UserException() :base ("User Exception") { }
        public UserException(string message) : base(message) { }
        public UserException(UserResult result, string message): base($"{(int)result} - {message}")
        {
            Result = result;
        }
        public UserException(UserResult result, HttpStatusCode httpStatusCode, string message) : this(result, $"{(int)result} - {message}")
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
