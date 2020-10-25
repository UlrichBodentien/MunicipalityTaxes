using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Dtos;

namespace MunicipalityTaxes.Core.Repositories.Tax
{
    public interface IMunicipalityTaxRepository
    {
        Task<Guid> AddAsync(MunicipalityTaxDto createMunicipalityTaxDto);

        Task<bool> AddRangeAsync(List<MunicipalityTaxDto> municipalityTaxes);

        Task<MunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate);

        Task<bool> UpdateAsync(MunicipalityTaxDto municipalityTaxDto);
    }
}