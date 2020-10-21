using System;

namespace MunicipalityTaxes.DataAccess.Model
{
    public class MunicipalityTax
    {
        public Guid Id { get; set; }

        public double Tax { get; set; }

        public DateTime StartDate { get; set; }

        public MunicipalityTaxTypeEnum TaxTypeId { get; set; }

        public Guid MunicipalityId { get; set; }

        public MunicipalityTaxType TaxType { get; set; }

        public Municipality Municipality { get; set; }
    }
}
