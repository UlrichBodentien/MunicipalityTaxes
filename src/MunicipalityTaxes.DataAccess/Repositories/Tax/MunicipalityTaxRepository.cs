using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.DataAccess.Exceptions;
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

        public async Task<MunicipalityTaxDto> GetAsync(string municipalityName, DateTime startDate)
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

                return new MunicipalityTaxDto
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

        public async Task<Guid> AddAsync(string municipalityName, MunicipalityTaxDto createMunicipalityTaxDto)
        {
            return await AddWithRetryAsync(municipalityName, createMunicipalityTaxDto);
        }

        private async Task<Guid> AddWithRetryAsync(string municipalityName, MunicipalityTaxDto createMunicipalityTaxDto, int retries = 0)
        {
            try
            {
                var municipality = await databaseContext.Municipality.FirstOrDefaultAsync(x => x.Name == municipalityName);
                if (municipality == null)
                {
                    throw new UnableToAddException(HttpStatusCode.BadRequest, "Municipality doesn't exist");
                }

                if (databaseContext.MunicipalityTax.Any(x => x.TaxType.Id == createMunicipalityTaxDto.TaxType
                    && x.StartDate == createMunicipalityTaxDto.StartDate
                    && x.MunicipalityId == municipality.Id))
                {
                    throw new UnableToAddException(HttpStatusCode.BadRequest, "Tax record already exists");
                }

                if (createMunicipalityTaxDto.TaxType.IsStartDateValid(createMunicipalityTaxDto.StartDate) == false)
                {
                    throw new UnableToAddException(HttpStatusCode.BadRequest, "Start date must match the selected tax type");
                }

                var muncipalityTax = new MunicipalityTax
                {
                    Id = Guid.NewGuid(),
                    StartDate = createMunicipalityTaxDto.StartDate,
                    Tax = createMunicipalityTaxDto.Tax,
                    TaxTypeId = createMunicipalityTaxDto.TaxType,
                    MunicipalityId = municipality.Id,
                };

                await databaseContext.AddAsync(muncipalityTax);
                await databaseContext.SaveChangesAsync();
                return muncipalityTax.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (retries == 0)
                {
                    retries++;
                    return await AddAsync(municipalityName, createMunicipalityTaxDto);
                }

                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to create the municipality tax");
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to create the municipality tax");
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
    }
}
