using System;
using MunicipalityTaxes.Core.Model;
using Newtonsoft.Json;

namespace MunicipalityTaxes.Core.Dtos
{
    public class MunicipalityTaxDto
    {
        [JsonProperty("tax")]
        public double Tax { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("taxType")]
        public MunicipalityTaxTypeEnum TaxType { get; set; }

        [JsonProperty("municipalityName")]
        public string MunicipalityName { get; set; }
    }
}
