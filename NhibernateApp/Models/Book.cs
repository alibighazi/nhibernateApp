using NhibernateApp.DbContext;

namespace NhibernateApp.Models
{
    public class Book : BaseModel<int> , IAuditable
    {
        public virtual string Title { get; set; }
    }
}
