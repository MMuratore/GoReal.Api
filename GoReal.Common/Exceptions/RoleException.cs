using GoReal.Common.Exceptions.Enumerations;
using System;

namespace GoReal.Common.Exceptions
{
    public class RoleException : Exception
    {
        public RoleResult Result { get; set; }

        public RoleException() :base ("User Exception") { }
        public RoleException(string message) : base(message) { }
        public RoleException(RoleResult result, string message): base($"{(int)result} - {message}")
        {
            Result = result;
        }
    }
}
