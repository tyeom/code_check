namespace Intercepting_Method_RealProxy
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public abstract class RuntimeProxyInvoker
    {
        public readonly object Target;
        public readonly MethodInfo Method;
        public readonly ReadOnlyCollection<object> Arguments;

        public RuntimeProxyInvoker(object target, MethodInfo method, object[] args)
        {
            this.Target = target;
            this.Method = method;
            this.Arguments = new ReadOnlyCollection<object>(args);
        }

        public object Invoke()
        {
            return Invoke(this.Target);
        }

        public object Invoke(object target)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            try
            {
                return this.Method.Invoke(target, this.Arguments.ToArray());
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
