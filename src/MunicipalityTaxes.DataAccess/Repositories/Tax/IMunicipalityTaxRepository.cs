using System;
using System.Threading.Tasks;
using MunicipalityTaxes.DataAccess.Dtos;

namespace MunicipalityTaxes.DataAccess.Repositories.Tax
{
    public interface IMunicipalityTaxRepository
    {
        Task<Guid> AddAsync(string municipalityName, MunicipalityTaxDto createMunicipalityTaxDto);

        Task<MunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate);
    }
}