using System;
using System.Threading.Tasks;
using MunicipalityTaxes.DataAccess.Dtos;

namespace MunicipalityTaxes.DataAccess.Repositories.Tax
{
    public interface IMunicipalityTaxRepository
    {
        Task<AddResult> AddAsync(string municipalityName, CreateMunicipalityTaxDto createMunicipalityTaxDto);

        Task<GetMunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate);
    }
}