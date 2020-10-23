using System.Collections.Generic;
using System.IO;
using MunicipalityTaxes.DataAccess.Dtos;

namespace MunicipalityTaxes.Core.Data
{
    public interface ICsvTaxParser
    {
        List<(string Name, MunicipalityTaxDto TaxDto)> ParseTaxCsvFile(Stream stream);
    }
}