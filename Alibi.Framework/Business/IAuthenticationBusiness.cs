using Alibi.Framework.Models;

namespace Alibi.Framework.Business
{
    public interface IAuthenticationBusiness
    {

        UserModel Login(string username, string password);
        UserModel Register(UserModel model);
    }
}
