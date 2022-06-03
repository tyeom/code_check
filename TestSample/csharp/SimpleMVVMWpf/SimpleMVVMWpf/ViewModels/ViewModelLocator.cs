namespace SimpleMVVMWpf.ViewModels
{
    using SimpleMVVMWpf.Common;
    using SimpleMVVMWpf.Services;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ※ 참고!
    /// 해당 ViewModelLocator클래스는 특정 ViewModel을 한번만 사용하기 위한 방법중 가장 간단한 방법을 기술해 놓은것일 뿐
    /// 실제 기타 라이브러리의 ViewModelLocator의 사용 방법 및 매니커즘과는 다르다.
    /// </summary>
    public class ViewModelLocator : SimpleIoC
    {
        private Dictionary<Type, object> _locatorFactoryDic = new Dictionary<Type, object>();

        public ViewModelLocator()
        {
            this.RegisterFactory<MainViewModel>();

            base.Register<IDBService, DBService>();

            base.RegisterSingleton<Test111>(() => new Test111());
            //base.Register<Test111>(() => new Test111());
        }

        public MainViewModel MainViewModel
        {
            get
            {
                if(_locatorFactoryDic.ContainsKey(typeof(MainViewModel)) &&
                    _locatorFactoryDic[typeof(MainViewModel)] == null)
                {
                    MainViewModel viewModel = (MainViewModel)base.Resolve(typeof(MainViewModel));
                    _locatorFactoryDic[typeof(MainViewModel)] = viewModel;
                }
                else if(_locatorFactoryDic.ContainsKey(typeof(MainViewModel)) &&
                    _locatorFactoryDic[typeof(MainViewModel)] != null)
                {
                    return (MainViewModel)_locatorFactoryDic[typeof(MainViewModel)];
                }

                return (MainViewModel)_locatorFactoryDic[typeof(MainViewModel)];
            }
        }

        public DataEditPageViewModel DataEditPageViewModel
        {
            get
            {
                return (DataEditPageViewModel)base.Resolve(typeof(DataEditPageViewModel));
            }
        }

        public APageViewModel APageViewModel
        {
            get
            {
                return (APageViewModel)base.Resolve(typeof(APageViewModel));
            }
        }

        public CPageViewModel CPageViewModel
        {
            get
            {
                return (CPageViewModel)base.Resolve(typeof(CPageViewModel));
            }
        }

        public DPageViewModel DPageViewModel
        {
            get
            {
                return (DPageViewModel)base.Resolve(typeof(DPageViewModel));
            }
        }

        public void RegisterFactory<TLocator>()
        {
            if (_locatorFactoryDic.ContainsKey(typeof(TLocator)) == false)
                _locatorFactoryDic.Add(typeof(TLocator), null);
        }

        public void InitFactory<TLocator>()
        {
            if (_locatorFactoryDic.ContainsKey(typeof(TLocator)))
                _locatorFactoryDic[typeof(TLocator)] = null;
        }
    }


    public class Test111
    {
        public Test111()
        {
            //
        }

        public string a { get; set; }
    }
}
