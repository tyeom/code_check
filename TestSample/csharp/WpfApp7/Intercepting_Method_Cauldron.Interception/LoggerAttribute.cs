namespace Intercepting_Method_Cauldron.Interception
{
    using Cauldron.Interception;
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class LoggerAttribute : Attribute, IMethodInterceptor
    {
        private string _methodName;

        public void OnEnter(Type declaringType, object instance, MethodBase methodbase, object[] values)
        {
            _methodName = methodbase.Name;
            this.AppendToFile($"Enter -> {declaringType.Name} {methodbase.Name} {string.Join(" ", values)}");
        }

        public void OnException(Exception e) => this.AppendToFile($"Exception -> {e.Message}");

        public void OnExit() => this.AppendToFile($"Exit -> {_methodName}");

        private void AppendToFile(string line)
        {
            Console.WriteLine("[Log] >> " + line);
        }
    }
}
