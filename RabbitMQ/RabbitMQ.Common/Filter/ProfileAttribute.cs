using AspectCore.DynamicProxy;
using CoreProfiler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Common.Filter
{
    public class ProfileAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var stepName = GetStepName(context);
            using (ProfilingSession.Current.Step(stepName))
            {
                await next(context);

            }

        }

        private string GetStepName(AspectContext context)
        {
            var implement = context.Implementation.GetType().Name;
            var method = context.ProxyMethod.Name;
            return $"{implement}-{method}";
        }
    }
}
