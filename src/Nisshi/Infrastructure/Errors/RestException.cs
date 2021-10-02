using System;
using System.Net;

namespace Nisshi.Infrastructure.Errors
{
    /// <summary>
    /// Exception encountered during the invoking of a REST endpoint
    /// </summary>
    public class RestException : Exception
    {
        public object Errors { get; set; } 
        public HttpStatusCode Code { get; }

        public RestException(HttpStatusCode code, object errors = null)
        {
            Errors = errors;
            Code = code;
        }
    }
}