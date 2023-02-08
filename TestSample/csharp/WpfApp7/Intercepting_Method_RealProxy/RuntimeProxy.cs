namespace Intercepting_Method_RealProxy
{
    using System;
    using System.Runtime.Remoting.Proxies;
    using System.Runtime.Remoting.Messaging;
    using System.Reflection;

    public abstract class RuntimeProxy
    {
        public static readonly object Default = new object();

        public static Target Create<Target>(Target instance, Func<RuntimeProxyInvoker, object> factory) where Target : class
        {
            return (Target)new InternalProxy<Target>(instance, new InternalRuntimeProxyInterceptor(factory)).GetTransparentProxy();
        }


        class InternalProxy<Target> : RealProxy where Target : class
        {
            readonly object Instance;
            readonly RuntimeProxyInterceptor Interceptor;

            public InternalProxy(Target instance, RuntimeProxyInterceptor interceptor)
                : base(typeof(Target))
            {
                Instance = instance;
                Interceptor = interceptor;
            }

            public override IMessage Invoke(IMessage msg)
            {
                var methodCall = (IMethodCallMessage)msg;
                var method = (MethodInfo)methodCall.MethodBase;

                try
                {
                    // Proxy를 통해 메세지 요청이 오면 Interceptor Func 호출
                    var result = Interceptor.Invoke(new InternalRuntimeProxyInterceptorInvoker(Instance, method, methodCall.InArgs));

                    if (result == RuntimeProxy.Default)
                        result = method.ReturnType.IsPrimitive ? Activator.CreateInstance(method.ReturnType) : null;

                    return new ReturnMessage(result, null, 0, methodCall.LogicalCallContext, methodCall);
                }
                catch (Exception ex)
                {
                    if (ex is TargetInvocationException && ex.InnerException != null)
                        return new ReturnMessage(ex.InnerException, msg as IMethodCallMessage);

                    return new ReturnMessage(ex, msg as IMethodCallMessage);
                }
            }
        }

        class InternalRuntimeProxyInterceptor : RuntimeProxyInterceptor
        {
            readonly Func<RuntimeProxyInvoker, object> Factory;

            public InternalRuntimeProxyInterceptor(Func<RuntimeProxyInvoker, object> factory)
            {
                this.Factory = factory;
            }

            public override object Invoke(RuntimeProxyInvoker invoker)
            {
                return Factory(invoker);
            }
        }

        class InternalRuntimeProxyInterceptorInvoker : RuntimeProxyInvoker
        {
            public InternalRuntimeProxyInterceptorInvoker(object target, MethodInfo method, object[] args)
                : base(target, method, args)
            { }
        }
    }
}
