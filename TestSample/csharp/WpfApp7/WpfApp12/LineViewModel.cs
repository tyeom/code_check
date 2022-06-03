using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp12
{
    public class LineViewModel : INotifyPropertyChanged
    {
        public LineViewModel()
        {
            //var pClass = IoC.Get<LinePointsViewModel>();
            Lines = new ObservableCollection<LinePointsViewModel>();
        }

        private ObservableCollection<LinePointsViewModel> _lines;

        public ObservableCollection<LinePointsViewModel> Lines
        {
            get { return _lines; }
            set
            {
                _lines = value;
                OnPropertyChanged();
            }
        }

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
