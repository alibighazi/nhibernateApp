using Alibi.Framework.Models;

namespace Server.Modules.Core.Common.Models
{
    public class Book : BaseModel<int>
    {
        public virtual string Title { get; set; }
    }
}
