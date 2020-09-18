using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class RoleException : Exception
    {
        public RoleResult Result { get; }
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;

        public RoleException() :base ("User Exception") { }
        public RoleException(string message) : base(message) { }
        public RoleException(RoleResult result, string message): base($"{(int)result} - {message}")
        {
            Result = result;
        }
        public RoleException(RoleResult result, HttpStatusCode httpStatusCode, string message) : this(result, $"{(int)result} - {message}")
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
