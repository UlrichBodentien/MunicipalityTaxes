using System;
using MunicipalityTaxes.DataAccess.Model;
using Newtonsoft.Json;

namespace MunicipalityTaxes.DataAccess.Dtos
{
    public class MunicipalityTaxDto
    {
        [JsonProperty("tax")]
        public double Tax { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("taxType")]
        public MunicipalityTaxTypeEnum TaxType { get; set; }
    }
}
