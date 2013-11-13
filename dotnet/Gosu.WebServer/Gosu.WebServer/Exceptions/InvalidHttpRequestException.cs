using System;

namespace Gosu.WebServer.Exceptions
{
    public class InvalidHttpRequestException : Exception
    {
        public InvalidHttpRequestException()
        {
        }

        public InvalidHttpRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}