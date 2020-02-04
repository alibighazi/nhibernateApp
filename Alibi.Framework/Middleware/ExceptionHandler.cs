using Alibi.Framework.Models;
using Alibi.Framework.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Alibi.Framework.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (exception.GetType().Name == "CustomValidationException")
            {
                var exceptionD = JsonConvert.DeserializeObject<Result>(exception.Message);
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    // customize as you need
                    Exception = new ExceptionModel
                    {
                        Code = "VALIDATION_ERROR",
                        Message = exceptionD.Message,
                        Type = exception.GetType().FullName,
                        Errors = exceptionD.Errors.Select(x => new { x.PropertyName, x.ErrorMessage })
                    }
                }));
            }
            else
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    Exception = new ExceptionModel
                    {
                        Code = "INTERNAL_ERROR",
                        Message = exception.Message,
                        Type = exception.GetType().FullName
                    }
                }));
            }
        }
    }
}