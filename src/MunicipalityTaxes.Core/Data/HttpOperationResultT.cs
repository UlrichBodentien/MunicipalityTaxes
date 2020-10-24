using MunicipalityTaxes.Core.Dtos;

namespace MunicipalityTaxes.Core.Data
{
    public class HttpOperationResult<T> : HttpOperationResult
    {
        public T Result { get; private set; }

        public static HttpOperationResult<T> Success(T result)
        {
            return new HttpOperationResult<T>
            {
                Result = result,
            };
        }

        public static HttpOperationResult<T> WrapError(HttpOperationResult httpOperationResult)
        {
            return new HttpOperationResult<T>
            {
                ErrorModel = httpOperationResult.ErrorModel,
                IsNetworkError = httpOperationResult.IsNetworkError,
            };
        }
    }
}
