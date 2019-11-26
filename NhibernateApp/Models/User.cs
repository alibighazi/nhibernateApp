namespace NhibernateApp.Models
{
    public class User: BaseModel<int>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string Token { get; set; }


    }
}
