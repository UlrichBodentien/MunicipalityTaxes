using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Dtos;
using Newtonsoft.Json;

namespace MunicipalityTaxes.Core.Data
{
    public class HttpRequestExecutor : IHttpRequestExecutor
    {
        private readonly HttpClient httpClient;

        public HttpRequestExecutor(string baseUri)
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri);
        }

        public async Task<HttpOperationResult<T>> GetAsync<T>(string relativeUri)
        {
            try
            {
                var response = await httpClient.GetAsync(relativeUri);
                if (response.IsSuccessStatusCode == false)
                {
                    var operationResult = await HandleNonSuccessStatusCodeAsync(response);
                    return HttpOperationResult<T>.WrapError(operationResult);
                }

                var result = await ParseResponseContentAsync<T>(response.Content);
                return HttpOperationResult<T>.Success(result);
            }
            catch (Exception)
            {
                return HttpOperationResult<T>.WrapError(
                    HttpOperationResult<T>.NetworkError());
            }
        }

        public async Task<HttpOperationResult> PostAsync<T>(string relativeUri, T dto)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(relativeUri, content);
                if (response.IsSuccessStatusCode == false)
                {
                    return await HandleNonSuccessStatusCodeAsync(response);
                }

                return HttpOperationResult.Success();
            }
            catch (Exception)
            {
                return HttpOperationResult<T>.NetworkError();
            }
        }

        public async Task<HttpOperationResult> PostAsync(string relativeUri, Stream csv)
        {
            try
            {
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(csv);
                content.Add(streamContent, "file", "file");

                var response = await httpClient.PostAsync(relativeUri, content);
                if (response.IsSuccessStatusCode == false)
                {
                    return await HandleNonSuccessStatusCodeAsync(response);
                }

                return HttpOperationResult.Success();
            }
            catch (Exception)
            {
                return HttpOperationResult.NetworkError();
            }
        }

        private async Task<HttpOperationResult> HandleNonSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            var errorModel = await CreateErrorModelAsync(response);
            return HttpOperationResult.Failed(errorModel);
        }

        private async Task<ErrorResponse> CreateErrorModelAsync(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new ErrorResponse
                {
                    Message = "Resource not found",
                };
            }

            return await ParseResponseContentAsync<ErrorResponse>(response.Content);
        }

        private async Task<T> ParseResponseContentAsync<T>(HttpContent content)
        {
            try
            {
                var serializer = new JsonSerializer();

                using var responseStream = await content.ReadAsStreamAsync();
                using var streamReader = new StreamReader(responseStream);
                using var jsonReader = new JsonTextReader(streamReader);

                return serializer.Deserialize<T>(jsonReader);
            }
            catch (JsonReaderException)
            {
                return default;
            }
        }
    }
}
