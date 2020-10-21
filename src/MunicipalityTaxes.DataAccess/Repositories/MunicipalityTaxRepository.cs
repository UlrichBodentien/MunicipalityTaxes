using System;
using System.Threading.Tasks;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.DataAccess.Repositories
{
    public class MunicipalityTaxRepository : IMunicipalityTaxRepository
    {
        private readonly MunicipalityContext databaseContext;

        public MunicipalityTaxRepository(MunicipalityContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<Guid> AddAsync(CreateMunicipalityTaxDto createMunicipalityTaxDto)
        {
            try
            {
                var muncipalityTax = new MunicipalityTax
                {
                    Id = Guid.NewGuid(),
                    StartDate = createMunicipalityTaxDto.StartDate,
                    Tax = createMunicipalityTaxDto.Tax,
                    TaxTypeId = createMunicipalityTaxDto.MunicipalityTaxType,
                    MunicipalityId = createMunicipalityTaxDto.MunicipalityId,
                };

                await databaseContext.AddAsync(muncipalityTax);
                await databaseContext.SaveChangesAsync();
                return muncipalityTax.Id;
            }
            catch (Exception)
            {
                return Guid.Empty;
            }
        }
    }
}
