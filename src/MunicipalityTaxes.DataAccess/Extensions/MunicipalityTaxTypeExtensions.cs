using System;
using MunicipalityTaxes.DataAccess.Model;

namespace MunicipalityTaxes.DataAccess.Extensions
{
    public static class MunicipalityTaxTypeExtensions
    {
        public static bool IsStartDateValid(this MunicipalityTaxTypeEnum municipalityTaxType, DateTime startDate)
        {
            if (municipalityTaxType == MunicipalityTaxTypeEnum.Yearly &&
                startDate.DayOfYear != 1)
            {
                return false;
            }

            if (municipalityTaxType == MunicipalityTaxTypeEnum.Monthly &&
                startDate.Day != 1)
            {
                return false;
            }

            if (municipalityTaxType == MunicipalityTaxTypeEnum.Weekly &&
                (int)startDate.DayOfWeek != 1)
            {
                return false;
            }

            return true;
        }
    }
}
