using Alibi.Framework.Validation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Alibi.Framework.Middlewares
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if(exception.GetType().Name == "ValidationException")
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = false,
                };
                
                var exceptionD = JsonConvert.DeserializeObject<Result>(exception.Message);
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    // customize as you need
                    error = new
                    {
                        message = exceptionD.Message,
                        exception = exception.GetType().Name,
                        Errors = exceptionD.Errors.Select(x => new { x.PropertyName , x.ErrorMessage})
                    }
                }));
            }
            else
            {
                await response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    // customize as you need
                    error = new
                    {
                        message = exception.Message,
                        exception = exception.GetType().Name
                    }
                }));
            }
            
        }
    }
}
