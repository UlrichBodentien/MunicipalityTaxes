using System.IO;
using System.Threading.Tasks;

namespace MunicipalityTaxes.Core.Data
{
    public interface IHttpRequestExecutor
    {
        Task<HttpOperationResult<T>> GetAsync<T>(string relativeUri);

        Task<HttpOperationResult> PostAsync<T>(string relativeUri, T dto);

        Task<HttpOperationResult> PostAsync(string relativeUri, Stream csv);
    }
}