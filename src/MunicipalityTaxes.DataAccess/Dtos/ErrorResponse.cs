using Newtonsoft.Json;

namespace MunicipalityTaxes.DataAccess.Dtos
{
    public class ErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
