using System;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.DataAccess.Dtos
{
    public class GetMunicipalityTaxDto
    {
        public double Tax { get; set; }

        public DateTime StartDate { get; set; }

        public MunicipalityTaxTypeEnum TaxType { get; set; }
    }
}
