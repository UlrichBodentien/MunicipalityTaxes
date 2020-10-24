using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using MunicipalityTaxes.Core.Dtos;
using MunicipalityTaxes.Core.Exceptions;
using MunicipalityTaxes.Core.Model;

namespace MunicipalityTaxes.Core.Data
{
    public class CsvTaxParser : ICsvTaxParser
    {
        private const string Delimitter = ",";

        public List<MunicipalityTaxDto> ParseTaxCsvFile(Stream stream)
        {
            var items = new List<MunicipalityTaxDto>();

            using var parser = PrepareParser(stream);
            while (parser.EndOfData == false)
            {
                var fields = parser.ReadFields();
                var record = ParseRecord(fields);

                items.Add(record);
            }

            return items;
        }

        private static MunicipalityTaxDto ParseRecord(string[] fields)
        {
            if (fields.Count() != 4)
            {
                throw new UnableToParseCsvException("Unable to parse row");
            }

            var name = fields[0];
            var taxTypeString = fields[1];
            var taxString = fields[2];
            var startDateString = fields[3];

            if (Enum.TryParse<MunicipalityTaxTypeEnum>(taxTypeString, out var taxType) == false)
            {
                throw new UnableToParseCsvException("Unable to parse tax type");
            }

            if (double.TryParse(taxString, NumberStyles.Float, CultureInfo.InvariantCulture, out var tax) == false)
            {
                throw new UnableToParseCsvException("Unable to parse tax");
            }

            if (DateTime.TryParse(startDateString, CultureInfo.InvariantCulture, DateTimeStyles.None, out var startDate) == false)
            {
                throw new UnableToParseCsvException("Unable to parse start date");
            }

            var record = new MunicipalityTaxDto
            {
                TaxType = taxType,
                Tax = tax,
                StartDate = startDate,
                MunicipalityName = name,
            };

            return record;
        }

        private static TextFieldParser PrepareParser(Stream stream)
        {
            var parser = new TextFieldParser(stream);
            parser.SetDelimiters(Delimitter);

            // Skip column names initially
            parser.ReadLine();

            return parser;
        }
    }
}
