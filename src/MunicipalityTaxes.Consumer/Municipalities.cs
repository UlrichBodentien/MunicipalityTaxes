using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MunicipalityTaxes.Core.Data;

namespace MunicipalityTaxes.Consumer
{
    public class Municipalities
    {
        private readonly IHttpRequestExecutor httpRequestExecutor;

        public Municipalities(IHttpRequestExecutor httpRequestExecutor)
        {
            this.httpRequestExecutor = httpRequestExecutor;
        }

        public async Task<bool> UploadMunicipalityDataFileAsync(Stream csv)
        {
            var result = await httpRequestExecutor.PostAsync($"api/municipalitytax/csvImport", csv);
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
