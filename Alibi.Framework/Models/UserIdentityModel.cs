using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alibi.Framework.Models
{
    public class UserIdentityModel : BaseModel<int>
    {

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Token { get; set; }
        public virtual List<Dictionary<string ,string>> Params { get; set; }
    
    }
}
