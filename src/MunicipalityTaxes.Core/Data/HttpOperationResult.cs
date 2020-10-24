using MunicipalityTaxes.Core.Dtos;

namespace MunicipalityTaxes.Core.Data
{
    public class HttpOperationResult
    {
        protected HttpOperationResult()
        {
        }

        public ErrorResponse ErrorModel { get; protected set; }

        public bool IsNetworkError { get; protected set; }

        public static HttpOperationResult Success()
        {
            return new HttpOperationResult
            {
            };
        }

        public static HttpOperationResult Failed(ErrorResponse errorModel)
        {
            return new HttpOperationResult
            {
                ErrorModel = errorModel,
            };
        }

        public static HttpOperationResult NetworkError()
        {
            return new HttpOperationResult
            {
                IsNetworkError = true,
            };
        }
    }
}
