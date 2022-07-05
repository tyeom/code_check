using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private static ThreadLocal<int> _tl = new ThreadLocal<int>();

        private static AsyncLocal<int> _al = new AsyncLocal<int>();

        private async Task WorkAsync(int i)
        {
            if (_al.Value == 0)
            {
                _al.Value = i;
                Console.WriteLine($"[새로운 값] Thread Id : {Environment.CurrentManagedThreadId} - tl Value : {_al.Value}");
            }
            else
            {
                Console.WriteLine($"[기존 값] Thread Id : {Environment.CurrentManagedThreadId} - tl Value : {_al.Value}");
            }

            await Task.Delay(1000);

            Console.WriteLine($"[await 이후 값] Thread Id : {Environment.CurrentManagedThreadId} - tl Value : {_al.Value}");

            Console.WriteLine("-----------------------------------------");
        }

        private async Task<string> QQQQ()
        {
            string result = await Task.Run<string>(() =>
            {
                Console.WriteLine($"Thread Id : {Environment.CurrentManagedThreadId}");

                System.Threading.Thread.Sleep(2000);

                Console.WriteLine($"Thread Id : {Environment.CurrentManagedThreadId}");
                return "aaa";
            });

            await Task.Delay(1000);

            Console.WriteLine($"Thread Id : {Environment.CurrentManagedThreadId}");

            return result;
        }

        public async void TTT()
        {
            await Task.Delay(7000);
        }

        private System.Windows.Forms.Timer dt = new System.Windows.Forms.Timer();
        private void Countdown(int count, int interval, Action<int> ts)
        {
            dt.Interval = interval;
            dt.Tick += (_, a) =>
            {
                if (count-- == 0)
                {
                    ts(count);
                    dt.Stop();
                }
            };

            dt.Start();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Countdown(20, 1000, cur =>
            {
                MessageBox.Show("dfgdfg");
            });

            TTT();
            MessageBox.Show("tttt");

            return;

            SynchronizationContext sc = SynchronizationContext.Current;
            Task.Run<string>(() =>
            {
                System.Threading.Thread.Sleep(2000);
                return "test";
            })
            .ContinueWith(t =>
            {
                sc.Post((p) =>
                {
                    string result = t.Result;
                    this.button1.Text = result;
                }, null);
            });

            //string aaa = await this.QQQQ();
            //this.button1.Text = aaa;

            return;

            // ThreadPool의 개수를 4개로 임의 제한
            //ThreadPool.SetMinThreads(4, 4);
            //ThreadPool.SetMaxThreads(4, 4);

            for (int i = 1; i < 11; i++)
            {
                await Task.Run(() => WorkAsync(i));
            }
        }



        public string GetDataSync(string param)
        {
            Console.WriteLine($"IsThreadPoolThread : {Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(5000);
            return $"{param} server data";
        }

        Func<string, string> getDataFunc;

        private IAsyncResult BeginGetData(string param, AsyncCallback asyncCallback, object state)
        {
            getDataFunc = this.GetDataSync;
            return getDataFunc.BeginInvoke(param, asyncCallback, this);
        }

        private string EndGetData(IAsyncResult asyncResult)
        {
            return getDataFunc.EndInvoke(asyncResult);
        }


        private void DataProcess_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine($"진행중...{e.ProgressPercentage}");
        }

        private void DataProcess_DataProcCompleted(object sender, DataProcCompletedEventArgs e)
        {
            if(e.Error == null)
            {
                Console.WriteLine($"완료 - 결과 : {e.Value} / {e.Result}");
            }
            else
            {
                Console.WriteLine($"Error 발생 : {e.Error.Message}");
            }
        }

        private Task<int> Output(string taskName, int i)
        {
            return new Task<int>(() =>
            {
                Console.WriteLine($"{taskName}");
                return i;
            });
        }

        private Task<bool> WorkAsync()
        {
            return Task.Run<bool>(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine(i);
                }
                return true;
            });
        }

        private async Task GetWorkResult()
        {
            Console.WriteLine("---------------------");
            Console.WriteLine("------------22222222---------");
            bool result = await this.WorkAsync();
            Console.WriteLine(result);
        }

        DataProcess dataProcess = null;
        Guid guid;
        private async void button2_Click(object sender, EventArgs e)
        {

            //Console.WriteLine("Logic1");
            //Console.WriteLine("Logic2");
            //await this.GetWorkResult();
            //Console.WriteLine("Logic3");

            //Task.Run<bool>(() =>
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        System.Threading.Thread.Sleep(1000);
            //        Console.WriteLine(i);
            //    }
            //    return true;
            //}).ContinueWith(t =>
            //{
            //    Console.WriteLine($"비동기 작업 결과 : {t.Result}");
            //});
            //Console.WriteLine("Task 비동기 시작!");



            //return;

            dataProcess = new DataProcess();
            dataProcess.ProgressChanged += this.DataProcess_ProgressChanged;
            dataProcess.DataProcCompleted += this.DataProcess_DataProcCompleted;
            guid = Guid.NewGuid();
            dataProcess.ProcessAsync(10, guid);

            //return;

            //PrimeNumberCalculatorMain p = new PrimeNumberCalculatorMain();
            //p.ShowDialog();


            //return;

            //IAsyncResult result = this.BeginGetData("AAA", GetDataCallBack, "파라메터");
            //Console.WriteLine("BeginGetData 호출");
            //Console.WriteLine("대기");

            //while(result.IsCompleted == false)
            //{
            //    //Console.WriteLine("비동기 작업 실행중..");
            //}

            //result.AsyncWaitHandle.WaitOne();
            //Console.WriteLine("대기완료");
        }

        private void GetDataCallBack(IAsyncResult asyncResult)
        {
            if (asyncResult.AsyncState is Form2 form2)
            {
                string result = form2.EndGetData(asyncResult);
                Console.WriteLine(result);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataProcess.CancelAsync(guid);
        }

        private bool _isBusy = true;  // volatile 키워드로 최적화 X 처리 가능

        private async void button4_Click(object sender, EventArgs e)
        {
            //Task.Run(this.Worker);
            //Thread.Sleep(10);
            //_isBusy = false;

            //Task.Run(this.Worker);
            //await Task.Delay(10);
            //_isBusy = false;


            Task.Run(this.Worker);
            await Task.Delay(10);
            Volatile.Write(ref _isBusy, false);  // Volatile.Write() / Volatile.Read() 대신에 _isBusy 맴버 필드에 volatile키워드를 사용해도 된다.
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _isBusy = false;
        }


        private void Worker()
        {
            int count = 0;
            //while (_isBusy)
            //{
            //    count++;
            //}
            while (Volatile.Read(ref _isBusy) == true)
            {
                count++;
            }
            Console.WriteLine($"count : {count}");
        }
    }
}
