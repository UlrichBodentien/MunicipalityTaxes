using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using MunicipalityTaxes.Core.Exceptions;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.Core.Data
{
    public class CsvTaxParser : ICsvTaxParser
    {
        private const string Delimitter = ",";

        public List<(string Name, CreateMunicipalityTaxDto TaxDto)> ParseTaxCsvFile(Stream stream)
        {
            var items = new List<(string name, CreateMunicipalityTaxDto taxDto)>();

            var parser = PrepareParser(stream);
            while (parser.EndOfData == false)
            {
                var fields = parser.ReadFields();
                var record = ParseRecord(fields);

                items.Add(record);
            }

            return items;
        }

        private static (string Name, CreateMunicipalityTaxDto TaxDto) ParseRecord(string[] fields)
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

            if (double.TryParse(taxString, out var tax) == false)
            {
                throw new UnableToParseCsvException("Unable to parse tax");
            }

            if (DateTime.TryParse(startDateString, out var startDate) == false)
            {
                throw new UnableToParseCsvException("Unable to parse start date");
            }

            var record = new CreateMunicipalityTaxDto
            {
                MunicipalityTaxType = taxType,
                Tax = tax,
                StartDate = startDate,
            };

            return (name, record);
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
