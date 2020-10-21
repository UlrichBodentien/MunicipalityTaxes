using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalityTaxes.DataAccess.Model
{
    public class Municipality
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<MunicipalityTax> Taxes { get; set; }
    }
}
