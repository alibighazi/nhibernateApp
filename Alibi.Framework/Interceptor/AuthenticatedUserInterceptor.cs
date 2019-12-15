using Alibi.Framework.DbContext;
using Alibi.Framework.Models;
using Alibi.Framework.Validation;
using Castle.DynamicProxy;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

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
            invocation.Arguments[0].GetType().GetProperty("Owner").SetValue(invocation.Arguments[0], user, null);


            var pps = invocation.Arguments[0].GetType().GetProperty("Value").PropertyType.GetProperties();

            foreach (var item in pps)
            {
                var propType = item.PropertyType;
                if (IsSubclassOfRawGeneric(typeof(Validator<>),propType))
                {
                    var validatorInstance = Activator.CreateInstance(propType);
                    var method = propType.GetMethods().Where(x => x.Name == "Validate").FirstOrDefault(x => x.IsPublic);
                    var methodParams = invocation.Arguments[0].GetType().GetProperty("Value").GetValue(invocation.Arguments[0], null);
                    var res = (Result)method.Invoke(validatorInstance, new object[] { methodParams });
                    var isValid = (bool)res.GetType().GetProperty("Succeeded").GetValue(res, null);
                    if(isValid == false)
                    {
                        throw new ValidationException(JsonSerializer.Serialize(res));
                    }

                }
            }

            
            invocation.Proceed();

        }


        private bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

    }

    public class ValidationException : Exception
    {
        public ValidationException(string resultat)
           : base(resultat)
        {
        }
    }
}