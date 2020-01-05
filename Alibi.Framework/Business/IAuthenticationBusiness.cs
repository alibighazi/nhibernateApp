using Alibi.Framework.Models;

namespace Alibi.Framework.Business
{
    public interface IAuthenticationBusiness
    {
        UserIdentityModel Login(string username, string password);
        UserIdentityModel Register(UserIdentityModel model);
    }
}