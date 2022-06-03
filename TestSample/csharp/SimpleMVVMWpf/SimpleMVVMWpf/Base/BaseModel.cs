namespace SimpleMVVMWpf.Base
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class BaseModel : INotifyPropertyChanged
    {
        #region Member Fields
        //
        #endregion

        #region Constructor
        public BaseModel() { }
        #endregion

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChange Implementation
    }

    public class BaseListModel<T> : List<T>, INotifyPropertyChanged
    {
        #region Member Fields
        //
        #endregion

        #region Constructor
        public BaseListModel() { }
        #endregion

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChange Implementation
    }
}
