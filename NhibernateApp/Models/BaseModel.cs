namespace NhibernateApp.Models
{
    public class BaseModel<T>
    {
        public virtual T Id { get; set; }

    }
}
