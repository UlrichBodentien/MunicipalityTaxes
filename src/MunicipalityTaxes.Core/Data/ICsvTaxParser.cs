using System.Collections.Generic;
using System.IO;
using MunicipalityTaxes.Core.Dtos;

namespace MunicipalityTaxes.Core.Data
{
    public interface ICsvTaxParser
    {
        List<MunicipalityTaxDto> ParseTaxCsvFile(Stream stream);
    }
}