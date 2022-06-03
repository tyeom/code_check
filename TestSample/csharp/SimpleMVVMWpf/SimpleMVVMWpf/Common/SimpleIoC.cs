namespace SimpleMVVMWpf.Common
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using SimpleMVVMWpf.Services;

    /// <summary>
    /// ※ 참고!
    /// 해당 IoC 클래스는 아주 기본적인 기능만 구현 되어 있는 클래스이다.
    /// 여기서는 생성되는 Instance가 싱글턴으로 관리 될지 여부, 퍼포먼스 등 기타 제대로 운영할 수 있는 코드는 제외되어 있으며, 참고용도로 봐야 한다.
    /// </summary>
    //public class SimpleIoC
    //{
    //    private Dictionary<Type, Func<object>> _factoryDic = new Dictionary<Type, Func<object>>();

    //    public SimpleIoC()
    //    {
    //        this.Register<IDBService, DBService>();
    //    }

    //    public void Register<TService, TImplementation>() where TImplementation : TService
    //    {
    //        if (_factoryDic.ContainsKey(typeof(TService)) == false)
    //        {
    //            _factoryDic.Add(typeof(TService),
    //                () => this.Resolve(typeof(TImplementation)));
    //        }
    //    }

    //    public object Resolve(Type key)
    //    {
    //        if (_factoryDic.ContainsKey(key))
    //        {
    //            var instance = _factoryDic[key];
    //            return instance.DynamicInvoke();
    //        }

    //        var ctor = key.GetConstructors().Single();
    //        var ctorParamTypes = ctor.GetParameters().Select(p => p.ParameterType).ToArray();
    //        var paramInstanceList = new List<Object>();
    //        ctorParamTypes.ToList().ForEach(item => paramInstanceList.Add(Resolve(item)));

    //        //return ctor.Invoke(ctorParamTypes);
    //        return Activator.CreateInstance(key, paramInstanceList.ToArray());
    //    }
    //}





    public class SimpleIoC
    {
        Dictionary<Type, Func<object>> _registrations = new Dictionary<Type, Func<object>>();

        public void Register<TService, TImpl>() where TImpl : TService
        {
            _registrations.Add(typeof(TService), () => this.Resolve(typeof(TImpl)));
        }

        public void Register<TService>(Func<TService> instanceCreator)
        {
            _registrations.Add(typeof(TService), () => instanceCreator());
        }

        public void RegisterSingleton<TService>(TService instance)
        {
            _registrations.Add(typeof(TService), () => instance);
        }

        /// <summary>
        /// 싱글톤으로 등록
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instanceCreator"></param>
        public void RegisterSingleton<TService>(Func<TService> instanceCreator)
        {
            var lazy = new Lazy<TService>(instanceCreator);
            this.Register<TService>(() => lazy.Value);
        }

        public object Resolve(Type serviceType)
        {
            Func<object> creator;
            if (_registrations.TryGetValue(serviceType, out creator)) return creator();
            else if (!serviceType.IsAbstract) return this.CreateInstance(serviceType);
            else throw new InvalidOperationException("No registration for " + serviceType);
        }

        private object CreateInstance(Type implementationType)
        {
            var ctor = implementationType.GetConstructors().Single();
            var parameterTypes = ctor.GetParameters().Select(p => p.ParameterType);
            var dependencies = parameterTypes.Select(t => this.Resolve(t)).ToArray();
            object instance = Activator.CreateInstance(implementationType, dependencies);

            return instance;
        }

        public void Release()
        {
            _registrations.Clear();
        }
    }
}
