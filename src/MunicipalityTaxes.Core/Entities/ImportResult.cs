using System.Collections.Generic;
using MunicipalityTaxes.Core.Data;

namespace MunicipalityTaxes.Core.Entities
{
    public class ImportResult
    {
        private ImportResult()
        {
            ErrorMessage = string.Empty;
        }

        public bool DidSucceed { get; set; }

        public bool IsBadFormat { get; set; }

        public string ErrorMessage { get; private set; }

        public static ImportResult BadFormat(string errorMessage)
        {
            return new ImportResult
            {
                IsBadFormat = true,
                ErrorMessage = errorMessage,
            };
        }

        public static ImportResult Success()
        {
            return new ImportResult
            {
                DidSucceed = true,
            };
        }
    }
}
