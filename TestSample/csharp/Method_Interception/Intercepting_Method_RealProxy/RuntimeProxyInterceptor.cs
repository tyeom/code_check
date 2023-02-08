namespace Intercepting_Method_RealProxy
{
    public abstract class RuntimeProxyInterceptor
    {
        public virtual object Invoke(RuntimeProxyInvoker invoker)
        {
            return invoker.Invoke();
        }
    }
}
