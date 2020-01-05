using Alibi.Framework.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alibi.Framework.BaseHttp
{
    public class BaseResponse<T>
    {
        private IList<T> Values { get; set; }

        public int CountItems { get; set; }
        public bool Success { get; set; }


        [JsonIgnore]
        public System.Exception Exception
        {
            set
            {
                var ex = value;
                Errors = new ExceptionModel
                {
                    Code = "INTERNAL_ERROR",
                    Message = ex.Message,
                    Type = ex.GetType().FullName
                };
                Success = false;
            }
        }

        private ExceptionModel Errors { get; set; }


        protected BaseResponse()
        {
            Values = new List<T>();
            Success = true;
        }
    }
}