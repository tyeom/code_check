using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

namespace WpfApp7
{
    public class ViewModelBase : CustomTypeDescriptor, INotifyPropertyChanged
    {
        List<PropertyDescriptor> _properties = new List<PropertyDescriptor>();

        protected void SetProBidning<T>(T model)
        {
            Type t = model.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach(var prop in props)
            {
                BindingProAttribute att = prop.GetCustomAttribute<BindingProAttribute>();
                if (att == null) continue;

                this.AddProperty(prop.PropertyType, prop.Name);
            }
        }

        protected void AddProperty(Type type, string propertyName)
        {
            var customProperty =
                    new CustomPropertyDescriptor(
                                            propertyName,
                                            type);

            _properties.Add(customProperty);
            customProperty.AddValueChanged(
                                        this,
                                        (o, e) => { OnPropertyChanged(propertyName); });
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            var properties = base.GetProperties();
            return new PropertyDescriptorCollection(
                                properties.Cast<PropertyDescriptor>()
                                          .Concat(_properties).ToArray());
        }

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChange Implementation
    }

    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        private Type _componentType;
        private object _value;

        #region Constructor
        public CustomPropertyDescriptor(string propertyName, Type componentType)
            : base(propertyName, new Attribute[] { })
        {
            _componentType = componentType;
        }
        #endregion

        #region PropertyDescriptor Implementation Overriden Methods
        public override Type ComponentType => _componentType;

        public override bool IsReadOnly => false;

        public override Type PropertyType => _componentType;

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return _value;
        }

        public override void ResetValue(object component)
        {
            SetValue(component, _componentType);
        }

        public override void SetValue(object component, object value)
        {
            _value = value;
            OnValueChanged(component, new EventArgs());
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
        #endregion
    }

    public class BindingProAttribute : Attribute
    {
        public BindingProAttribute()
        {
            
        }
    }

    public class TestModel : ViewModelBase
    {
        public TestModel()
        {
            base.SetProBidning<TestModel>(this);
        }

        [BindingPro]
        public string AA { get; set; }

        public string BB { get; set; }

        [BindingPro]
        public string CC { get; set; }
    }

    public class QQQ
    {
        public string AA { get; set; }
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, ILayoutControl> _layoutControlList;

        public MainWindow()
        {
            InitializeComponent();

            List<QQQ> qqq = new List<QQQ>();
            qqq.Add(new WpfApp7.QQQ() { AA = "11111"});
            qqq.Add(new WpfApp7.QQQ() { AA = "222222" });
            qqq.Add(new WpfApp7.QQQ() { AA = "3333" });
            QQQ = qqq;

            this.DataContext = this;

            Test = new TestModel();

        }

        public List<QQQ> QQQ
        {
            get;
            set;
        }

        public TestModel Test
        {
            get;set;
        }

        public ILayoutControl HeaderControl
        {
            get; set;
        }

        public ILayoutControl InnerControl
        {
            get; set;
        }

        public ILayoutControl FooterControl
        {
            get; set;
        }

        public void 레이아웃초기화및구성(ILayoutControl layoutControl, string key)
        {
            _layoutControlList.Add(key, layoutControl);
        }

        private void GoBikeEndStoryboard_Completed(object sender, EventArgs e)
        {

        }

        private void xBikeLeft_Click(object sender, RoutedEventArgs e)
        {
            List<QQQ> qqq = new List<QQQ>();
            qqq.AddRange(QQQ);
            qqq.Add(null);
            qqq.Add(new WpfApp7.QQQ());
            QQQ = qqq;
            this.OnPropertyChanged("QQQ");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Test.AA = "22222";
            
        }

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChange Implementation
    }
}
