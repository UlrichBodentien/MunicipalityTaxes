using System;
using System.IO;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.Core.Model;

namespace MunicipalityTaxes.Consumer
{
    public class Program
    {
        private const string ApiUrl = "https://localhost:44320";
        private const string AssetsFolder = "Assets";

        private static IHttpRequestExecutor httpRequestExecutor;

        public static async Task Main(string[] args)
        {
            httpRequestExecutor = new HttpRequestExecutor(ApiUrl);

            Console.WriteLine("Welcome to municipality tax consumer");
            Console.WriteLine();
            Console.WriteLine($"There are 3 operations available {Environment.NewLine} 1: Create Municipality Tax {Environment.NewLine} 2: Upload csv file {Environment.NewLine} 3: Get tax for a municipality at a date");
            Console.WriteLine();

            var operation = Console.ReadLine();

            try
            {
                switch (operation)
                {
                    case "1":
                        await CreateMunicipalityTaxAsync(CreateMunicipality());
                        break;
                    case "2":
                        await UploadCsvFileAsync(CreateMunicipalies());
                        break;
                    case "3":
                        await GetTaxForMunicipalityAsync(CreateMunicipality());
                        break;
                    default:
                        Console.WriteLine("That's not a valid operation...");
                        break;
                }
            }
            catch (InvalidInputException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task CreateMunicipalityTaxAsync(IMunicipality municipality)
        {
            var date = GetDateTime();
            var tax = GetTax();
            var taxType = GetTaxType();

            try
            {
                await municipality.CreateTaxAsync(date, tax, taxType);
                Console.WriteLine($"Successfully created the tax record for {municipality.Name}");
            }
            catch (FailedTaxOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task UploadCsvFileAsync(Municipalities municipalities)
        {
            var csv = OpenFileStream("valid.csv");

            try
            {
                Console.WriteLine("Uploading csv data...");
                await municipalities.UploadMunicipalityDataFileAsync(csv);
                Console.WriteLine("Successfully imported the tax records");
            }
            catch (FailedTaxOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task GetTaxForMunicipalityAsync(IMunicipality municipality)
        {
            var date = GetDateTime();

            try
            {
                var tax = await municipality.GetTaxAsync(date);
                Console.WriteLine($"The tax for {municipality.Name} at {date:MM-dd-yyyy} is {tax}");
            }
            catch (FailedTaxOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IMunicipality CreateMunicipality()
        {
            var municipalityName = GetMunicipalityName();
            return new Municipality(httpRequestExecutor, municipalityName);
        }

        private static Municipalities CreateMunicipalies()
        {
            return new Municipalities(httpRequestExecutor);
        }

        private static string GetMunicipalityName()
        {
            Console.WriteLine("Please type the municipality name");
            var municipalityName = Console.ReadLine();
            if (string.IsNullOrEmpty(municipalityName))
            {
                throw new InvalidInputException("Invalid municipality specified");
            }

            return municipalityName;
        }

        private static DateTime GetDateTime()
        {
            Console.WriteLine("Please type the date");
            var dateString = Console.ReadLine();
            if (DateTime.TryParse(dateString, out var date) == false)
            {
                throw new InvalidInputException("Invalid date specified");
            }

            return date;
        }

        private static double GetTax()
        {
            Console.WriteLine("Please type the tax");
            var taxString = Console.ReadLine();
            if (double.TryParse(taxString, out var tax) == false)
            {
                throw new InvalidInputException("Invalid tax specified");
            }

            return tax;
        }

        private static MunicipalityTaxTypeEnum GetTaxType()
        {
            Console.WriteLine("Please type the tax type (either Daily, Weekly, Monthly or Yearly)");
            var taxTypeString = Console.ReadLine();
            if (Enum.TryParse<MunicipalityTaxTypeEnum>(taxTypeString, out var taxType) == false)
            {
                throw new InvalidInputException("Invalid tax type specified");
            }

            return taxType;
        }

        private static FileStream OpenFileStream(string filePath)
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(directory, AssetsFolder, filePath);

            return File.OpenRead(path);
        }
    }
}
