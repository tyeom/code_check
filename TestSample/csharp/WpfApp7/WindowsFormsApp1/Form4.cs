using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        private Timer _tmr;
        (string, string)[] _종목배열 = { ("A0001", "삼성전자"), ("A0002", "카카오"), ("A0003", "NAVER"), ("A0004", "SK바이오닉스") };

        public Form4()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;

            _tmr = new Timer();
            _tmr.Interval = 1000;
            _tmr.Tick += (s, _) =>
            {
                // 임의로 1초마다 데이터가 새로 들어오고 갱신된다.
                Random rnd = new Random(DateTime.Now.Second);
                int rndNum = rnd.Next(0, 4);
                (string, string) 임의종목 = _종목배열[rndNum];

                // DataList에 추가된 종목인지 체크
                var 종목 = DataList.FirstOrDefault(p => p.종목코드 == 임의종목.Item1);
                // 새로운 종목 데이터면 추가
                if(종목 == null)
                {
                    DataList.Add(new Data() { 종목코드 = 임의종목.Item1, 종목명 = 임의종목.Item2, 현재가 = new Random(DateTime.Now.Second).Next(1000, 9999), 변동률 = new Random(DateTime.Now.Second).Next(1, 100) });
                }
                // 기존에 추가된 종목 데이터면 업데이트
                else
                {
                    종목.현재가 = new Random(DateTime.Now.Second).Next(1000, 9999);
                    종목.변동률 = new Random(DateTime.Now.Second).Next(1, 100);
                }
            };
        }

        public BindingList<Data> DataList
        {
            get;
            set;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView a;
           

            DataList = new BindingList<Data>();
            this.dataGridView1.DataSource = DataList;

            DataList.AddNew();
            _tmr.Start();
        }
    }

    public class Data : INotifyPropertyChanged
    {
        private string _종목코드;
        private string _종목명;
        private Int64 _현재가;
        private double _변동률;

        public string 종목코드
        {
            get => _종목코드;
            set
            {
                _종목코드 = value;
                OnPropertyChanged();
            }
        }

        public string 종목명
        {
            get => _종목명;
            set
            {
                _종목명 = value;
                OnPropertyChanged();
            }
        }

        public Int64 현재가
        {
            get => _현재가;
            set
            {
                _현재가 = value;
                OnPropertyChanged();
            }
        }

        public double 변동률
        {
            get => _변동률;
            set
            {
                _변동률 = value;
                OnPropertyChanged();
            }
        }

        #region INotifyPropertyChange Implementation
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion INotifyPropertyChange Implementation
    }
}
