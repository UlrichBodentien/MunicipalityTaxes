using Newtonsoft.Json;

namespace MunicipalityTaxes.Core.Dtos
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
