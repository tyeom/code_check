using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Transparent_Window
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ClearAllPropertyChangedHandlers()
        {
            if (this.PropertyChanged == null)
            {
                return;
            }

            foreach (PropertyChangedEventHandler handler in this.PropertyChanged.GetInvocationList())
            {
                this.PropertyChanged -= handler;
            }
        }
        #endregion
    }
}
