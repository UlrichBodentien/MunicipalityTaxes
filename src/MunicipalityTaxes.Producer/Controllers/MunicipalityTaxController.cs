using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.Core.Dtos;
using MunicipalityTaxes.Core.Repositories.Tax;

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

        [HttpGet]
        [Route("{municipalityName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string municipalityName, [FromQuery] DateTime date)
        {
            var tax = await municipalityTaxRepository.GetAsync(municipalityName, date);
            if (tax == null)
            {
                return NotFound();
            }

            return Ok(tax);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] MunicipalityTaxDto createMunicipalityTaxDto)
        {
            var id = await municipalityTaxRepository.AddAsync(createMunicipalityTaxDto);
            return Ok(id);
        }

        [HttpPost]
        [Route("csvImport")]
        public async Task<IActionResult> ImportCsvDataAsync([FromForm(Name = "file")] IFormFile file)
        {
            using var stream = file.OpenReadStream();

            var records = csvTaxParser.ParseTaxCsvFile(stream);
            foreach (var record in records)
            {
                await municipalityTaxRepository.AddAsync(record);
            }

            return Ok(file.Name);
        }
    }
}
