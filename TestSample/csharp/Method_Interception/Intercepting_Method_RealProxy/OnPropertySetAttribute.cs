namespace Intercepting_Method_Cauldron.Interception
{
    using Cauldron.Interception;
    using System;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class OnPropertySetAttribute : Attribute, IPropertySetterInterceptor
    {
        [AssignMethod("{CtorArgument:0}")]
        public Action<string, object> _onSetMethod = null;

        public OnPropertySetAttribute(string methodName)
        {
            
        }

        public void OnException(Exception e)
        {
        }

        public void OnExit()
        {
        }

        public bool OnSet(PropertyInterceptionInfo propertyInterceptionInfo, object oldValue, object newValue)
        {
            this._onSetMethod?.Invoke(propertyInterceptionInfo.PropertyName, newValue);
            return false;
        }
    }
}
