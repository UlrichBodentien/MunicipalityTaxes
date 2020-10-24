using System;

namespace MunicipalityTaxes.Consumer
{
    internal class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string message)
            : base(message)
        {
        }
    }
}