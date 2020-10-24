using System;

namespace MunicipalityTaxes.Consumer
{
    internal class FailedTaxOperationException : Exception
    {
        public FailedTaxOperationException()
        {
        }

        public FailedTaxOperationException(string message)
            : base(message)
        {
        }
    }
}