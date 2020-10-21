using System;
using System.Threading.Tasks;
using MunicipalityTaxes.DataAccess.Dtos;

namespace MunicipalityTaxes.DataAccess.Repositories
{
    public interface IMunicipalityTaxRepository
    {
        Task<Guid> AddAsync(CreateMunicipalityTaxDto createMunicipalityTaxDto);
    }
}