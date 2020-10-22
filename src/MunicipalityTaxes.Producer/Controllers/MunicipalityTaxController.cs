using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.Core.Exceptions;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Repositories.Tax;

namespace MunicipalityTaxes.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalityTaxController : ControllerBase
    {
        private readonly IMunicipalityTaxRepository municipalityTaxRepository;
        private readonly ICsvTaxParser csvTaxParser;

        public MunicipalityTaxController(
            IMunicipalityTaxRepository municipalityTaxRepository,
            ICsvTaxParser csvTaxParser)
        {
            this.municipalityTaxRepository = municipalityTaxRepository;
            this.csvTaxParser = csvTaxParser;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateMunicipalityTaxDto createMunicipalityTaxDto)
        {
            var result = await municipalityTaxRepository.AddAsync(createMunicipalityTaxDto);
            if (result.DidSucceed == false)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Id);
        }

        [HttpPost]
        [Route("csvImport")]
        public async Task<IActionResult> ImportCsvDataAsync([FromForm(Name = "file")] IFormFile file)
        {
            using var stream = file.OpenReadStream();

            try
            {
                var records = csvTaxParser.ParseTaxCsvFile(stream);
                foreach (var record in records)
                {
                    await municipalityTaxRepository.AddAsync(record);
                }
            }
            catch (UnableToParseCsvException)
            {
                return BadRequest("Bad file format. Unable to parse the csv file");
            }

            return Ok(file.Name);
        }
    }
}
