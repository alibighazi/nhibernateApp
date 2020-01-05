using System.Collections.Generic;

namespace Alibi.Framework.Models
{
    public class UserIdentityModel : Entity
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Token { get; set; }
        public virtual List<Dictionary<string, string>> Params { get; set; }
    }
}