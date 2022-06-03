using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConsoleApp3
{
    class Test
    {
        public void Call(Action<string> callback)
        {
            callback(System.IO.Path.GetRandomFileName());
        }
    }

    class Process
    {
        private readonly ManualResetEventSlim _processResetEvent = new ManualResetEventSlim(false);

        public async Task<string> Login(string id, string pw)
        {
            string data = null;
            await Task.Run(() =>
            {
                Test t = new Test();
                t.Call((_) =>
                {
                    data = _;
                    //_processResetEvent.Set();
                });
            });

            //_processResetEvent.Wait();

            return data;
        }

        private void CallBack(string data)
        {
            Console.WriteLine(data);
        }
    }

    class Program
    {
        private async void QQQ()
        {
            Thread.Sleep(5000);
        }

        private void QQQ1()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("QQQ1");
                }
            }
            finally
            {
                Console.WriteLine("QQQ - finally");
            }
        }

        private static void Logical_Test(object objContext)
        {
            var data1 = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("key1");
            var data2 = System.Runtime.Remoting.Messaging.CallContext.GetData("key11");
        }

        static AsyncLocal<int> _asyncLocal = new AsyncLocal<int>();
        static ThreadLocal<int> _threadLocal = new ThreadLocal<int>();

        static string str = "a";

        private static async Task Output(int num)
        {
            await Task.Yield();
            //Console.WriteLine($"num : {num} / thread id : {Thread.CurrentThread.ManagedThreadId} / asyncLocal : {_asyncLocal.Value} / threadLocal : {_threadLocal.Value}");

            //await Task.Delay(1000);

            Console.WriteLine($"num : {num} / thread id : {Thread.CurrentThread.ManagedThreadId} / 변경 전 asyncLocal : {_asyncLocal.Value} / 변경 전 threadLocal : {_threadLocal.Value}");

            _asyncLocal.Value = num;
            _threadLocal.Value = num;

            Console.WriteLine($"num : {num} / thread id : {Thread.CurrentThread.ManagedThreadId} / asyncLocal : {_asyncLocal.Value} / threadLocal : {_threadLocal.Value}");
        }

        static async Task Main(string[] args)
        {
            Task task1 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(10000);
                Console.WriteLine("AAA");
            });

            Task task2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("BBB");
                //Thread.Sleep(3000);
            });

            Task.WaitAll(task1, task2);

            Console.WriteLine("2222");

            Console.ReadLine();

            _asyncLocal.Value = -1;
            _threadLocal.Value = -1;

            var tasks = Enumerable.Range(0, 10).Select(p => Output(p));
            await Task.WhenAll(tasks);

            Console.WriteLine($"thread id : {Thread.CurrentThread.ManagedThreadId} / asyncLocal : {_asyncLocal.Value} / threadLocal : {_threadLocal.Value}");

            Console.ReadLine();
        }

        private async void EntryAsyncVoid()
        {
            await Task.Run(() => this.Void());

            Console.WriteLine("Called AsyncVoid!!!!");
        }

        private async void EntryAsyncTask()
        {
            await Task.Run(() => this.Async());

            Console.WriteLine("Called AsyncTask!!!!");
        }

        private async void Void()
        {
            for (int i = 0; i < 5; i++)
            {
                var cap = i;
                await Task.Delay(1000);

                Console.WriteLine($"Call Void, count:{cap}");
            }
        }

        private async Task Async()
        {
            for (int i = 0; i < 5; i++)
            {
                var cap = i;
                await Task.Delay(1000);

                Console.WriteLine($"Call Async ~~~~~~~~~~~~~~~~  :{cap}");
            }
        }



        static void QQ()
        {
            Console.WriteLine("111a");
        }


        //Call();


        //Console.WriteLine("Function Main Run");
        //var c = new TestClass();
        //c.Run();
        //Console.WriteLine("Function Main End");

        //Console.ReadLine();

        static async void Call()
        {
            Console.WriteLine("aaaaaa");
            await Task.Run(TestFunc);
            Console.WriteLine("bbbbbb");
        }

        static void TestFunc()
        {
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(i);
            }
        }
    }

    public class Base
    {
        public string BaseValue { get; set; }
    }

    [XmlRoot(Namespace = "foo", ElementName = "RootElement", DataType = "string", IsNullable = true)]
    public class A : Base
    {
        public string AValue { get; set; }
    }

    [XmlRoot(Namespace = "foo", ElementName = "RootElement", DataType = "string", IsNullable = true)]
    public class B : Base
    {
        public string AValue { get; set; }
        public string BValue { get; set; }
    }
}
