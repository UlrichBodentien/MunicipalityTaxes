using System;
using MunicipalityTaxes.Core.Extensions;
using MunicipalityTaxes.DataAccess.Model;
using Xunit;

namespace MunicipalityTaxes.Core.Tests.Extensions
{
    public class MunicipalityTaxTypeExtensionsTests
    {
        public static readonly object[][] TestCases =
        {
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 1, 1), MunicipalityTaxType = MunicipalityTaxTypeEnum.Yearly, ExpectedResult = true } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 2, 2), MunicipalityTaxType = MunicipalityTaxTypeEnum.Yearly, ExpectedResult = false } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 1, 30), MunicipalityTaxType = MunicipalityTaxTypeEnum.Yearly, ExpectedResult = false } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 5, 1), MunicipalityTaxType = MunicipalityTaxTypeEnum.Monthly, ExpectedResult = true } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 2, 2), MunicipalityTaxType = MunicipalityTaxTypeEnum.Monthly, ExpectedResult = false } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 3, 30), MunicipalityTaxType = MunicipalityTaxTypeEnum.Monthly, ExpectedResult = false } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 1, 6), MunicipalityTaxType = MunicipalityTaxTypeEnum.Weekly, ExpectedResult = true } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 3, 30), MunicipalityTaxType = MunicipalityTaxTypeEnum.Weekly, ExpectedResult = true } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 1, 30), MunicipalityTaxType = MunicipalityTaxTypeEnum.Weekly, ExpectedResult = false } },
            new object[] { new MunicipalityTaxTypeExtensionsTestCase { StartDate = new DateTime(2020, 3, 31), MunicipalityTaxType = MunicipalityTaxTypeEnum.Weekly, ExpectedResult = false } },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void IsStartDateValid_ValidatesCorrectly(MunicipalityTaxTypeExtensionsTestCase testCase)
        {
            var result = testCase.MunicipalityTaxType.IsStartDateValid(testCase.StartDate);
            Assert.Equal(result, testCase.ExpectedResult);
        }

        public class MunicipalityTaxTypeExtensionsTestCase
        {
            public DateTime StartDate { get; set; }

            public MunicipalityTaxTypeEnum MunicipalityTaxType { get; set; }

            public bool ExpectedResult { get; set; }
        }
    }
}
