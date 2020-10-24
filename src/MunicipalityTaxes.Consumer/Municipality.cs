using System;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Data;
using MunicipalityTaxes.Core.Dtos;
using MunicipalityTaxes.Core.Model;

namespace MunicipalityTaxes.Consumer
{
    public class Municipality : IMunicipality
    {
        private readonly IHttpRequestExecutor httpRequestExecutor;

        public Municipality(IHttpRequestExecutor httpRequestExecutor, string name)
        {
            this.httpRequestExecutor = httpRequestExecutor;

            Name = name;
        }

        public string Name { get; }

        public async Task<double> GetTaxAsync(DateTime date)
        {
            var result = await httpRequestExecutor.GetAsync<MunicipalityTaxDto>($"api/municipalitytax/{Name}?date={date:MM-dd-yyyy}");
            if (TryHandleErrors(result, out var errorMessage))
            {
                throw new FailedTaxOperationException(errorMessage);
            }

            return result.Result.Tax;
        }

        public async Task<bool> CreateTaxAsync(DateTime date, double tax, MunicipalityTaxTypeEnum taxType)
        {
            var dto = new MunicipalityTaxDto
            {
                StartDate = date,
                Tax = tax,
                TaxType = taxType,
                MunicipalityName = Name,
            };

            var result = await httpRequestExecutor.PostAsync($"api/municipalitytax", dto);
            if (TryHandleErrors(result, out var errorMessage))
            {
                throw new FailedTaxOperationException(errorMessage);
            }

            return true;
        }

        private bool TryHandleErrors(HttpOperationResult result, out string errorMessage)
        {
            if (result.IsNetworkError)
            {
                errorMessage = "Oops, please check that you have access to the internet";
                return true;
            }

            if (result.ErrorModel != null)
            {
                errorMessage = $"Oops something went wrong: {result.ErrorModel.Message}";
                return true;
            }

            errorMessage = string.Empty;
            return false;
        }
    }
}
