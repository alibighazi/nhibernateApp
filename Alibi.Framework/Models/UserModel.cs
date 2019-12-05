namespace Alibi.Framework.Models
{
    public class UserModel : BaseModel<int>
    {

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Token { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }

    }
}
