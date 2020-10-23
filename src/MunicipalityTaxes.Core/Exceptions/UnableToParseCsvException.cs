using System.Net;

namespace MunicipalityTaxes.Core.Exceptions
{
    public class UnableToParseCsvException : HttpStatusException
    {
        public UnableToParseCsvException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
