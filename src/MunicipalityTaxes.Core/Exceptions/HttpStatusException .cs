using System;
using System.Net;

namespace MunicipalityTaxes.Core.Exceptions
{
    public class HttpStatusException : Exception
    {
        public HttpStatusException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
