using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MunicipalityTaxes.Core.Extensions;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Repositories;

namespace MunicipalityTaxes.Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunicipalityTaxController : ControllerBase
    {
        private readonly IMunicipalityTaxRepository municipalityTaxRepository;

        public MunicipalityTaxController(IMunicipalityTaxRepository municipalityTaxRepository)
        {
            this.municipalityTaxRepository = municipalityTaxRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]CreateMunicipalityTaxDto createMunicipalityTaxDto)
        {
            if (createMunicipalityTaxDto.MunicipalityTaxType.IsStartDateValid(createMunicipalityTaxDto.StartDate) == false)
            {
                return BadRequest("Start date must match the selected tax type");
            }

            var id = await municipalityTaxRepository.AddAsync(createMunicipalityTaxDto);
            if (id == Guid.Empty)
            {
                return Conflict("Could not create the MunicipalityTax");
            }

            return Ok(id);
        }
    }
}