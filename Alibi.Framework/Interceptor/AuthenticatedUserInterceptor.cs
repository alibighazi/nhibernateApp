using Alibi.Framework.BaseHttp;
using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Alibi.Framework.Interceptor
{
    public class AuthenticatedUserInterceptor : Castle.DynamicProxy.IInterceptor
    {

        private IHttpContextAccessor _accessor;
        private IRepository<UserIdentityModel> _repository;

        public AuthenticatedUserInterceptor(IHttpContextAccessor accessor, IRepository<UserIdentityModel> repository)
        {
            _accessor = accessor;
            _repository = repository;
        }



        public void Intercept(IInvocation invocation)
        {

            var claimsIdentity = _accessor.HttpContext.User.Identity as ClaimsIdentity;
                       
            var userId = int.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            var user = _repository.FindById(userId);

            var args = (BaseRequest)invocation.Arguments[0];

            args.Owner = user;

            invocation.Arguments[0] = args;

            invocation.Proceed();

        }
    }
}