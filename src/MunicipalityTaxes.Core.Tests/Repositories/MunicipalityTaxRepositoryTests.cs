using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunicipalityTaxes.Core.Dtos;
using MunicipalityTaxes.Core.Model;
using MunicipalityTaxes.Core.Repositories.Tax;
using Xunit;

namespace MunicipalityTaxes.Tests.Repositories
{
    public class MunicipalityTaxRepositoryTests
    {
        public static readonly object[][] TestCases =
        {
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Copenhagen", Date = new DateTime(2020, 1, 1), ExpectedResult = 0.1 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Copenhagen", Date = new DateTime(2020, 5, 2), ExpectedResult = 0.4 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Copenhagen", Date = new DateTime(2020, 7, 10), ExpectedResult = 0.2 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Copenhagen", Date = new DateTime(2020, 3, 16), ExpectedResult = 0.2 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Sorø", Date = new DateTime(2020, 7, 10), ExpectedResult = 0.22 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Sorø", Date = new DateTime(2020, 5, 2), ExpectedResult = 0.3 } },
            new object[] { new MunicipalityRepositoryTestCase { Municipality = "Sorø", Date = new DateTime(2020, 11, 4), ExpectedResult = 0.25 } },
        };

        private readonly MunicipalityContext context;
        private readonly MunicipalityTaxRepository repository;

        public MunicipalityTaxRepositoryTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MunicipalityContext>()
                .UseSqlServer($"Server=(local); Database={Guid.NewGuid().ToString()}; Trusted_connection=true");

            context = new MunicipalityContext(optionsBuilder.Options);
            context.Database.EnsureCreated();

            repository = new MunicipalityTaxRepository(context);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public async Task GetAsync_ReturnsExpectedRecord(MunicipalityRepositoryTestCase testCase)
        {
            await SetupTestTaxesAsync();

            var result = await repository.GetAsync(testCase.Municipality, testCase.Date);
            Assert.Equal(testCase.ExpectedResult, result.Tax);
        }

        private async Task SetupTestTaxesAsync()
        {
            var copenhagen = new Municipality
            {
                Id = Guid.NewGuid(),
                Name = "Copenhagen",
            };

            var soro = new Municipality
            {
                Id = Guid.NewGuid(),
                Name = "Sorø",
            };

            context.Municipality.Add(copenhagen);
            context.Municipality.Add(soro);
            await context.SaveChangesAsync();

            var taxes = GetTestTaxes();
            await repository.AddRangeAsync(taxes);
        }

        private static List<MunicipalityTaxDto> GetTestTaxes()
        {
            return new List<MunicipalityTaxDto>
            {
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Copenhagen",
                    StartDate = new DateTime(2020, 1, 1),
                    Tax = 0.2,
                    TaxType = MunicipalityTaxTypeEnum.Yearly,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Copenhagen",
                    StartDate = new DateTime(2020, 5, 1),
                    Tax = 0.4,
                    TaxType = MunicipalityTaxTypeEnum.Monthly,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Copenhagen",
                    StartDate = new DateTime(2020, 1, 1),
                    Tax = 0.1,
                    TaxType = MunicipalityTaxTypeEnum.Daily,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Copenhagen",
                    StartDate = new DateTime(2020, 12, 25),
                    Tax = 0.1,
                    TaxType = MunicipalityTaxTypeEnum.Daily,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Sorø",
                    StartDate = new DateTime(2020, 1, 1),
                    Tax = 0.22,
                    TaxType = MunicipalityTaxTypeEnum.Yearly,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Sorø",
                    StartDate = new DateTime(2020, 11, 2),
                    Tax = 0.25,
                    TaxType = MunicipalityTaxTypeEnum.Weekly,
                },
                new MunicipalityTaxDto
                {
                    Id = Guid.NewGuid(),
                    MunicipalityName = "Sorø",
                    StartDate = new DateTime(2020, 5, 2),
                    Tax = 0.3,
                    TaxType = MunicipalityTaxTypeEnum.Daily,
                },
            };
        }
    }
}
