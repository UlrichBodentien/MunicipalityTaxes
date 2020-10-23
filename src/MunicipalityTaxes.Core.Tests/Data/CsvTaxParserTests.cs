using System.IO;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.Core.Exceptions;
using Xunit;

namespace MunicipalityTaxes.Core.Tests.Data
{
    public class CsvTaxParserTests
    {
        private const string AssetsFolder = "Assets";

        private readonly CsvTaxParser csvTaxParser;

        public CsvTaxParserTests()
        {
            csvTaxParser = new CsvTaxParser();
        }

        [Fact]
        public void ParseCsvFile_ValidFile_ReturnsRecords()
        {
            using var fileStream = OpenFileStream("valid.csv");

            var result = csvTaxParser.ParseTaxCsvFile(fileStream);

            Assert.Equal(9, result.Count);
            Assert.Equal("Holbæk", result[0].MunicipalityName);
            Assert.Equal("Sorø", result[5].MunicipalityName);
        }

        [Fact]
        public void ParseCsvFile_MissingFields_ThrowsUnableToParseException()
        {
            using var fileStream = OpenFileStream("missingField.csv");
            Assert.Throws<UnableToParseCsvException>(() => csvTaxParser.ParseTaxCsvFile(fileStream));
        }

        [Fact]
        public void ParseCsvFile_FieldNotParseable_ThrowsUnableToParseException()
        {
            using var fileStream = OpenFileStream("fieldNotParseable.csv");
            Assert.Throws<UnableToParseCsvException>(() => csvTaxParser.ParseTaxCsvFile(fileStream));
        }

        private static FileStream OpenFileStream(string filePath)
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(directory, AssetsFolder, filePath);

            return File.OpenRead(path);
        }
    }
}
