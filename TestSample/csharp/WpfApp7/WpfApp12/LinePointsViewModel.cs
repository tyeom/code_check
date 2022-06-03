using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp12
{
    public class LinePointsViewModel : INotifyPropertyChanged
    {
        public LinePointsViewModel()
        {
            _linePoints = new PointCollection();
            _linePoints.Changed += CollectionChanged;
        }

        private void CollectionChanged(object sender, EventArgs e)
        {
            //내부적으로 포인트 추가 삭제, 변경이 있을 떄 PropertyChange하는 로직이 
            //있으면 좋겠는데... 
        }

        private PointCollection _linePoints;

        public PointCollection LinePoints
        {
            get { return _linePoints; }
            set
            {
                _linePoints = value;
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
