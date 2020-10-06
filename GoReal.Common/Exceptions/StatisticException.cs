using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class StatisticException : Exception
    {
        public StatisticResult Result { get; }
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;

        public StatisticException() : base("User Exception") { }
        public StatisticException(string message) : base(message) { }
        public StatisticException(StatisticResult result, string message) : base($"{(int)result} - {message}")
        {
            Result = result;
        }
        public StatisticException(StatisticResult result, HttpStatusCode httpStatusCode, string message) : this(result, $"{(int)result} - {message}")
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
