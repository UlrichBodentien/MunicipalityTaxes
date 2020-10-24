using System;
using System.IO;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Model;

namespace MunicipalityTaxes.Consumer
{
    public interface IMunicipality
    {
        string Name { get; }

        Task<double> GetTaxAsync(DateTime date);

        Task<bool> CreateTaxAsync(DateTime date, double tax, MunicipalityTaxTypeEnum taxType);
    }
}