using System;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Dtos;

namespace MunicipalityTaxes.Core.Repositories.Tax
{
    public interface IMunicipalityTaxRepository
    {
        Task<Guid> AddAsync(MunicipalityTaxDto createMunicipalityTaxDto);

        Task<MunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate);
    }
}