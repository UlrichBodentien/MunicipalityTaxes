using System.Net;

namespace MunicipalityTaxes.Utilities.Exceptions
{
    public class UnableToParseCsvException : HttpStatusException
    {
        public UnableToParseCsvException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }
    }
}
