﻿using System.Collections.Generic;
using System.IO;
using MunicipalityTaxes.DataAccess.Dtos;

namespace MunicipalityTaxes.Core.Data
{
    public interface ICsvTaxParser
    {
        List<(string Name, CreateMunicipalityTaxDto TaxDto)> ParseTaxCsvFile(Stream stream);
    }
}