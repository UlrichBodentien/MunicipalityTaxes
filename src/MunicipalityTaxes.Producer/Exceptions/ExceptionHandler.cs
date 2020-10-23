using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using MunicipalityTaxes.DataAccess.Dtos;
using MunicipalityTaxes.Utilities.Exceptions;
using Newtonsoft.Json;

namespace MunicipalityTaxes.Producer.Exceptions
{
    public class ExceptionHandler
    {
        public static async Task HandleExceptionAsync(HttpContext context)
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature.Error;

            context.Response.StatusCode = GetStatusCode(exception);

            var errorResponse = CreateErrorResponse(context, exception);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorResponse);
        }

        private static string CreateErrorResponse(HttpContext context, Exception exception)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.InternalServerError)
            {
                return JsonConvert.SerializeObject(new ErrorResponse { Message = "Ooops, we encountered an error" });
            }
            else
            {
                return JsonConvert.SerializeObject(new ErrorResponse { Message = exception.Message });
            }
        }

        private static int GetStatusCode(Exception exception)
        {
            if (exception is HttpStatusException httpStatusException)
            {
                return (int)httpStatusException.StatusCode;
            }

            return (int)HttpStatusCode.InternalServerError;
        }
    }
}
