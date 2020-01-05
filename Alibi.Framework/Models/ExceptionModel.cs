using System.Collections.Generic;

namespace Alibi.Framework.Models
{
    public class ExceptionModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public IEnumerable<object> Errors { get; set; }
    }
}