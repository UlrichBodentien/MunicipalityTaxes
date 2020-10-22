using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Extensions;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.DataAccess.Repositories.Tax
{
    public class MunicipalityTaxRepository : IMunicipalityTaxRepository
    {
        private readonly MunicipalityContext databaseContext;

        public MunicipalityTaxRepository(MunicipalityContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<AddResult> AddAsync(CreateMunicipalityTaxDto createMunicipalityTaxDto)
        {
            try
            {
                var municipality = await databaseContext.Municipality.FirstOrDefaultAsync(x => x.Name == createMunicipalityTaxDto.MunicipalityName);
                if (municipality == null)
                {
                    return AddResult.FailedResult("Municipality doesn't exist");
                }

                if (databaseContext.MunicipalityTax.Any(x => x.TaxType.Id == createMunicipalityTaxDto.MunicipalityTaxType
                    && x.StartDate == createMunicipalityTaxDto.StartDate
                    && x.MunicipalityId == municipality.Id))
                {
                    return AddResult.FailedResult("Tax record already exists");
                }

                if (createMunicipalityTaxDto.MunicipalityTaxType.IsStartDateValid(createMunicipalityTaxDto.StartDate) == false)
                {
                    return AddResult.FailedResult("Start date must match the selected tax type");
                }

                var muncipalityTax = new MunicipalityTax
                {
                    Id = Guid.NewGuid(),
                    StartDate = createMunicipalityTaxDto.StartDate,
                    Tax = createMunicipalityTaxDto.Tax,
                    TaxTypeId = createMunicipalityTaxDto.MunicipalityTaxType,
                    MunicipalityId = municipality.Id,
                };

                await databaseContext.AddAsync(muncipalityTax);
                await databaseContext.SaveChangesAsync();
                return AddResult.Success(muncipalityTax.Id);
            }
            catch (Exception)
            {
                return AddResult.FailedResult("Could not create the MunicipalityTax");
            }
        }
    }
}
