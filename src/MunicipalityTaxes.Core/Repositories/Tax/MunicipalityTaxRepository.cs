using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MunicipalityTaxes.Core.Dtos;
using MunicipalityTaxes.Core.Exceptions;
using MunicipalityTaxes.Core.Extensions;
using MunicipalityTaxes.Core.Model;

namespace MunicipalityTaxes.Core.Repositories.Tax
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

                if (taxRecord == null)
                {
                    return null;
                }

                return new MunicipalityTaxDto
                {
                    StartDate = taxRecord.StartDate,
                    Tax = taxRecord.Tax,
                    TaxType = taxRecord.TaxTypeId,
                    MunicipalityName = municipalityName,
                    Id = taxRecord.Id,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> AddRangeAsync(List<MunicipalityTaxDto> municipalityTaxes)
        {
            try
            {
                foreach (var dto in municipalityTaxes)
                {
                    var dataModel = await CreateDataModelAsync(dto);
                    await databaseContext.AddAsync(dataModel);
                }

                await databaseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                CheckIfDuplicate(ex, "One of the records already exists, or the list contains duplicates");
                throw new UnableToAddException(HttpStatusCode.BadRequest, "Unable to import the csv file");
            }
        }

        public async Task<Guid> AddAsync(MunicipalityTaxDto municipalityTaxDto)
        {
            return await AddWithRetryAsync(municipalityTaxDto);
        }

        public async Task<bool> UpdateAsync(MunicipalityTaxDto municipalityTaxDto)
        {
            return await UpdateWithRetryAsync(municipalityTaxDto);
        }

        private async Task<bool> UpdateWithRetryAsync(MunicipalityTaxDto municipalityTaxDto, int retries = 0)
        {
            try
            {
                var doesTaxExist = databaseContext.MunicipalityTax.Any(x => x.Id == municipalityTaxDto.Id);
                if (doesTaxExist == false)
                {
                    return false;
                }

                var dataModel = await CreateDataModelAsync(municipalityTaxDto);
                databaseContext.Attach(dataModel).State = EntityState.Modified;

                await databaseContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (retries == 0)
                {
                    return await UpdateWithRetryAsync(municipalityTaxDto, 1);
                }

                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to update the municipality tax");
            }
            catch (DbUpdateException)
            {
                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to update the municipality tax");
            }
        }

        private async Task<Guid> AddWithRetryAsync(MunicipalityTaxDto municipalityTaxDto, int retries = 0)
        {
            try
            {
                var muncipalityTax = await CreateDataModelAsync(municipalityTaxDto);

                await databaseContext.AddAsync(muncipalityTax);
                await databaseContext.SaveChangesAsync();
                return muncipalityTax.Id;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (retries == 0)
                {
                    return await AddWithRetryAsync(municipalityTaxDto, 1);
                }

                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to create the municipality tax");
            }
            catch (DbUpdateException ex)
            {
                CheckIfDuplicate(ex, "Tax record already exists");
                throw new UnableToAddException(HttpStatusCode.InternalServerError, "Unable to create the municipality tax");
            }
        }

        private async Task<MunicipalityTax> CreateDataModelAsync(MunicipalityTaxDto municipalityTaxDto)
        {
            var municipality = await databaseContext.Municipality.FirstOrDefaultAsync(x => x.Name == municipalityTaxDto.MunicipalityName);
            if (municipality == null)
            {
                throw new UnableToAddException(HttpStatusCode.BadRequest, "Municipality doesn't exist");
            }

            if (municipalityTaxDto.TaxType.IsStartDateValid(municipalityTaxDto.StartDate) == false)
            {
                throw new UnableToAddException(HttpStatusCode.BadRequest, "Start date must match the selected tax type");
            }

            var id = municipalityTaxDto.Id == Guid.Empty ? Guid.NewGuid() : municipalityTaxDto.Id;
            var muncipalityTax = new MunicipalityTax
            {
                Id = id,
                StartDate = municipalityTaxDto.StartDate,
                Tax = municipalityTaxDto.Tax,
                TaxTypeId = municipalityTaxDto.TaxType,
                MunicipalityId = municipality.Id,
            };

            return muncipalityTax;
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

        private void CheckIfDuplicate(DbUpdateException ex, string message)
        {
            var sqlException = ex.InnerException as SqlException;
            if (sqlException != null && sqlException.Errors.OfType<SqlError>()
                 .Any(se => se.Number == 2601 || se.Number == 2627))  /* PK or UKC violation */
            {
                throw new UnableToAddException(HttpStatusCode.BadRequest, message);
            }
        }
    }
}
