namespace NhibernateApp.Models
{
    public class User: BaseModel<int>
    {

        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string Fullname { get; set; }

    }
}
