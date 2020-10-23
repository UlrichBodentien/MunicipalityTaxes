using System.Net;
using MunicipalityTaxes.Utilities.Exceptions;

namespace MunicipalityTaxes.DataAccess.Exceptions
{
    public class UnableToAddException : HttpStatusException
    {
        public UnableToAddException(HttpStatusCode statusCode, string message)
            : base(statusCode, message)
        {
        }
    }
}
