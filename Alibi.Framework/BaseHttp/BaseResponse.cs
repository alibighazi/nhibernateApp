using System.Collections.Generic;

namespace Alibi.Framework.BaseHttp
{
    public class BaseResponse<T>
    {
        public IList<T> Values { get; set; }
    }
}
