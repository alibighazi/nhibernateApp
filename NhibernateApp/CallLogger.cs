using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace NhibernateApp
{
    public class CallLogger : IInterceptor
    {
        private readonly ILogger _logger;

        public CallLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("logger name here");
            _logger.LogInformation("helloghazi");
        }

        public void Intercept(IInvocation invocation)
        {
            _logger.LogInformation("Calling method {0} with parameters {1}... ",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            _logger.LogInformation("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}
