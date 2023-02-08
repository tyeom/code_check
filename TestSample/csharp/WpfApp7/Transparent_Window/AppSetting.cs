namespace Transparent_Window
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class AppSetting : NotifyPropertyChanged
    {
        private static readonly Lazy<AppSetting> lazy =
        new Lazy<AppSetting>(() => new AppSetting());
        /// <summary>
        /// 항상 위에 보이기 여부
        /// </summary>
        private bool _isTopMose = true;

        public static AppSetting Instance { get { return lazy.Value; } }

        /// <summary>
        /// 항상 위에 보이기 여부
        /// </summary>
        public bool IsTopMost
        {
            get => _isTopMose;
            set
            {
                if (_isTopMose != value)
                {
                    _isTopMose = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 마우스 이벤트 허용 여부
        /// </summary>
        public bool IsMouseEventMessagePass
        {
            get;set;
        }
    }
}
