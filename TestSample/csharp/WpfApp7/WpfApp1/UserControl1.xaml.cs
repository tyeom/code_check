using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

            this.DataContext = new ViewModel();
        }
    }

    public class ViewModel //: INotifyPropertyChanged
    {
        private string _sampleText = "메모리 누수";

        public string SampleText
        {
            get => _sampleText;
            set => _sampleText = value;
        }

        //public string SampleText
        //{
        //    get => _sampleText;
        //    set
        //    {
        //        _sampleText = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SampleText)));
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
