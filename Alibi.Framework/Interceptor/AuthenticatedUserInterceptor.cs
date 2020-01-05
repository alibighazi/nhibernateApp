using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Alibi.Framework.Interceptor
{
    public class AuthenticatedUserInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRepository<UserIdentityModel> _repository;

        public AuthenticatedUserInterceptor(IHttpContextAccessor accessor, IRepository<UserIdentityModel> repository)
        {
            _accessor = accessor;
            _repository = repository;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_accessor.HttpContext.User.Identity is ClaimsIdentity claimsIdentity)
            {
                var userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                invocation.Arguments[0].GetType().GetProperty("Owner")
                    ?.SetValue(invocation.Arguments[0], _repository.FindById(userId), null);
            }

            invocation.Proceed();
        }
    }
}