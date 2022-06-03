namespace Reversi
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class ReversiCellModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 0 = 빈칸 <para/>
        /// 1 = black <para/>
        /// 2 = white
        /// </summary>
        public int _cellState = 0;

        public int Idx
        {
            get;
            set;
        }

        public string CellText
        {
            get
            {
                if (CellState == 0)
                {
                    return string.Empty;
                }
                else if (CellState == 1)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
        }

        public int CellState
        {
            get { return _cellState; }
            set
            {
                _cellState = value;
                OnPropertyChanged();
                OnPropertyChanged("CellText");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
