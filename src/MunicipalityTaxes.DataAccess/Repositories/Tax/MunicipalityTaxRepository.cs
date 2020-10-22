using System;
using System.Collections;
using System.Collections.Generic;
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

        public async Task<GetMunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate)
        {
            try
            {
                var taxRecordsWithinYear = await databaseContext
                    .MunicipalityTax
                    .Where(x =>
                        x.Municipality.Name == municipalityName
                        && startDate.Year == x.StartDate.Year)
                    .ToListAsync();

                var taxRecord = taxRecordsWithinYear
                    .Where(x =>
                        startDate >= x.StartDate
                        && startDate < x.StartDate.AddDays(CalculateDaysToAdd(x.StartDate, x.TaxTypeId)))
                    .OrderBy(x => x.TaxTypeId)
                    .FirstOrDefault();

                return new GetMunicipalityTaxDto
                {
                    StartDate = taxRecord.StartDate,
                    Tax = taxRecord.Tax,
                    TaxType = taxRecord.TaxTypeId
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private int CalculateDaysToAdd(DateTime startDate, MunicipalityTaxTypeEnum taxTypeId)
        {
            return taxTypeId switch
            {
                MunicipalityTaxTypeEnum.Daily => 1,
                MunicipalityTaxTypeEnum.Weekly => 7,
                MunicipalityTaxTypeEnum.Monthly => DateTime.DaysInMonth(startDate.Year, startDate.Month),
                MunicipalityTaxTypeEnum.Yearly => new DateTime(startDate.Year, 12, 31).DayOfYear,
                _ => throw new NotImplementedException(),
            };
        }

        public async Task<AddResult> AddAsync(string municipalityName, CreateMunicipalityTaxDto createMunicipalityTaxDto)
        {
            try
            {
                var municipality = await databaseContext.Municipality.FirstOrDefaultAsync(x => x.Name == municipalityName);
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
