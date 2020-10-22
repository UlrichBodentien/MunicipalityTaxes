using System;

namespace MunicipalityTaxes.Core.Exceptions
{
    public class UnableToParseCsvException : Exception
    {
        public UnableToParseCsvException(string message)
            : base(message)
        {
        }
    }
}
