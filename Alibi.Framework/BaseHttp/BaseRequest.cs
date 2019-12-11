using Alibi.Framework.Models;

namespace Alibi.Framework.BaseHttp
{
    public class BaseRequest<T> : BaseRequest
    {
        public T Value { get; set; }
    }

    public class BaseRequest
    {
        public UserIdentityModel Owner { get; set; }
    }
}
