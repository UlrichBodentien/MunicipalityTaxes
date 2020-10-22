using System;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.DataAccess.Dtos
{
    public class CreateMunicipalityTaxDto
    {
        public double Tax { get; set; }

        public DateTime StartDate { get; set; }

        public MunicipalityTaxTypeEnum MunicipalityTaxType { get; set; }

        public string MunicipalityName { get; set; }
    }
}
