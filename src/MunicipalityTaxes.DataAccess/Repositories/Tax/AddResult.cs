using System;

namespace MunicipalityTaxes.DataAccess.Repositories.Tax
{
    public class AddResult
    {
        private AddResult()
        {
            ErrorMessage = string.Empty;
        }

        public bool DidSucceed { get; private set; }

        public Guid Id { get; private set; }

        public string ErrorMessage { get; private set; }

        public static AddResult Success(Guid id)
        {
            return new AddResult
            {
                DidSucceed = true,
                Id = id
            };
        }

        internal static AddResult FailedResult(string errorMessage)
        {
            return new AddResult
            {
                ErrorMessage = errorMessage,
            };
        }
    }
}
