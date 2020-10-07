using GoReal.Common.Exceptions.Enumerations;
using System;
using System.Net;

namespace GoReal.Common.Exceptions
{
    public class CommonException : Exception
    {
        public CommonResult Result { get; }
        public HttpStatusCode HttpStatusCode { get; } = HttpStatusCode.NotFound;

        public CommonException() : base("Common Exception") { }
        public CommonException(string message) : base(message) { }
        public CommonException(CommonResult result, string message) : base($"{(int)result} - {message}")
        {
            Result = result;
        }
        public CommonException(CommonResult result, HttpStatusCode httpStatusCode, string message) : this(result, $"{(int)result} - {message}")
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
