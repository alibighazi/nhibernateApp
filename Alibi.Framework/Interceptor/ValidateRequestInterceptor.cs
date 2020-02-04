using Autofac;
using Castle.DynamicProxy;
using FluentValidation;
using System.Text.Json;
using Alibi.Framework.Exception;

namespace Alibi.Framework.Interceptor
{
    public class ValidateRequestInterceptor : Castle.DynamicProxy.IInterceptor
    {
        private readonly ILifetimeScope _scope;

        public ValidateRequestInterceptor(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void Intercept(IInvocation invocation)
        {
            var baseEntityType = invocation.Arguments[0].GetType().BaseType?.GetGenericArguments()[0];

            var genericType = typeof(IValidator<>);

            var specificType = genericType.MakeGenericType(baseEntityType);

            var methodParams = invocation.Arguments[0].GetType().GetProperty("Value")
                ?.GetValue(invocation.Arguments[0], null);


            var validationClass = (IValidator)_scope.Resolve(specificType);


            var validationResult = validationClass.Validate(methodParams);

            if (validationResult.IsValid == false)
            {
                throw new CustomValidationException(JsonSerializer.Serialize(validationResult));
            }


            invocation.Proceed();
        }
    }
}