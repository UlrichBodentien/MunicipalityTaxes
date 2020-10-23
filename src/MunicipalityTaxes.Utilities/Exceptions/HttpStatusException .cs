using System;
using System.Net;

namespace MunicipalityTaxes.Utilities.Exceptions
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
