using System.Net;
using MunicipalityTaxes.Core.Exceptions;

namespace MunicipalityTaxes.Core.Exceptions
{
    public class UnableToAddException : HttpStatusException
    {
        public UnableToAddException(HttpStatusCode statusCode, string message)
            : base(statusCode, message)
        {
        }
    }
}
