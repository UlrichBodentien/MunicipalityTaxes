using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxes.Core.Data;
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

        [HttpGet]
        [Route("{municipalityName}/{date}")]
        public async Task<IActionResult> GetAsync([FromRoute]string municipalityName, [FromRoute]DateTime date)
        {
            var tax = await municipalityTaxRepository.GetAsync(municipalityName, date);
            if (tax == null)
            {
                return NotFound();
            }

            return Ok(tax);
        }

        [HttpPost("{municipalityName}")]
        public async Task<IActionResult> CreateAsync([FromRoute]string municipalityName, [FromBody]MunicipalityTaxDto createMunicipalityTaxDto)
        {
            var id = await municipalityTaxRepository.AddAsync(municipalityName, createMunicipalityTaxDto);
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
                await municipalityTaxRepository.AddAsync(record.Name, record.TaxDto);
            }

            return Ok(file.Name);
        }
    }
}
