using GoReal.Common.Exceptions.Enumerations;
using System;

namespace GoReal.Common.Exceptions
{
    public class UserException : Exception
    {
        public UserResult Result { get; set; }

        public UserException() :base ("User Exception") { }
        public UserException(string message) : base(message) { }
        public UserException(UserResult result, string message): base($"{(int)result} - {message}")
        {
            Result = result;
        }
    }
}
