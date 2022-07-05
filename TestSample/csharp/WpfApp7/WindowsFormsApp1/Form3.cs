using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public struct AsyncMyTaskMethodBuilder<T>
    {
        /// <summary>
        /// async 메서드 빌더 [컴파일러 자동 코드 생성]
        /// </summary>
        AsyncTaskMethodBuilder _methodBuilder;

        public MyTask<T> Task
        {
            get
            {
                return new MyTask<T>(_methodBuilder.Task);
            }
        }

        public static AsyncMyTaskMethodBuilder<T> Create()
        {
            return new AsyncMyTaskMethodBuilder<T> { _methodBuilder = AsyncTaskMethodBuilder.Create() };
        }

        public void SetException(Exception exception)
        {
            _methodBuilder.SetException(exception);
        }

        public void SetResult(T result)
        {
            _methodBuilder.SetResult();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            _methodBuilder.SetStateMachine(stateMachine);
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            _methodBuilder.Start(ref stateMachine);
        }
    }

    public struct MyTaskAwaiter<T> : ICriticalNotifyCompletion, INotifyCompletion
    {
        private readonly MyTask<T> _t;

        public MyTaskAwaiter(MyTask<T> t)
        {
            _t = t;
        }

        public bool IsCompleted => _t.MyTaskCompleted;

        public T GetResult()
        {
            return _t.MyTaskResult;
        }

        // OnCompleted메서드로 처리 되는 경우
        // CallContext의 LogicalCallContext 데이터가 새로운 스레드 풀에서 생성된 스레드에 이관 된다.
        public void OnCompleted(Action continuation)
        {
            if (_t.Task == null)
            {
                _t.AddContinuation(continuation);
            }
            else
            {
                _t.Task.GetAwaiter().OnCompleted(continuation);
            }
        }

        // UnsafeOnCompleted메서드로 OnCompleted처리 되는 경우
        // CallContext의 LogicalCallContext 데이터가 새로운 스레드 풀에서 생성된 스레드에 이관 되지 않는다.
        public void UnsafeOnCompleted(Action continuation)
        {
            if (_t.Task == null)
            {
                _t.AddContinuation(continuation);
            }
            else
            {
                _t.Task.GetAwaiter().UnsafeOnCompleted(continuation);
            }
        }
    }

    [AsyncMethodBuilder(typeof(AsyncMyTaskMethodBuilder<>))]
    public class MyTask<T>
    {
        private Task _task;
        private Thread _thread;
        private Func<T> _action;
        private List<Action> _continuationActionList = new List<Action>();
        private bool _myTaskCompleted;

        public MyTask(Func<T> action)
        {
            _action = action;
            _thread = new Thread(new ThreadStart(this.ExecuteAction));
            _thread.Start();
        }

        public MyTask(Task task)
        {
            _task = task;
        }

        internal Task Task
        {
            get { return _task; }
        }

        public bool MyTaskCompleted
        {
            get
            {
                if (_thread == null)
                {
                    return _task.IsCompleted;
                }

                return _myTaskCompleted;
            }
        }

        public T MyTaskResult { get; private set; }

        public void AddContinuation(Action action)
        {
            _continuationActionList.Add(action);
        }

        public MyTaskAwaiter<T> GetAwaiter()
        {
            return new MyTaskAwaiter<T>(this);
        }

        private void ExecuteAction()
        {
            T result = default(T);
            if (_action != null)
                result = _action();

            MyTaskResult = result;
            _myTaskCompleted = true;

            foreach (var continuationAction in _continuationActionList)
            {
                continuationAction();
            }
        }
    }

    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private async Task<string> Foo()
        {
            //await Task.Run(() =>
            //{
            //    Thread.Sleep(4000);
            //    return "aaaa";
            //}).ConfigureAwait(false);

            //int a = 11;

            await Task.Yield();

            return "aaawwww";

            return await Task.Run(() =>
            {
                Thread.Sleep(4000);
                return "aaaa";
            });
        }

        private async MyTask<string> AsyncProcess()
        {
            return await new MyTask<string>(() =>
            {
                Thread.Sleep(5000);
                return "오래 걸리는 작업";
            });
        }

        //private async Task<string> AsyncProcess()
        //{
        //    return await Task.Run(() =>
        //    {
        //        Thread.Sleep(5000);
        //        return "오래 걸리는 작업";
        //    });
        //}



        //private async void button1_Click(object sender, EventArgs e)
        //{
        //    var aa = await AsyncProcess();
        //    MessageBox.Show(aa);
        //}

        private async void button1_Click(object sender, EventArgs e)
        {
            var t = await this.Foo();
            //string aaa = await t;
            //string bbb = await this.qq();

            using (Form5 f5 = new Form5())
            {
                f5.ShowDialog();
            }
        }

        private async void f1()
        {
            for (int i = 0; i < 100; i++)
            {
                var cap = i;
                //await Task.Delay(1000);

                Console.WriteLine($"Call Void, count:{cap}");
            }
        }

        private async void f2()
        {
            Console.WriteLine("f2 end");
        }

        private async void Async_f1()
        {
            await Task.Run(() => this.f1());

            Console.WriteLine("Async_f1 호출함!");
        }

        private async void Async_f2()
        {
            this.f2();

            Console.WriteLine("Async_f2 호출함!");
        }

        private async void EntryAsyncVoid()
        {
            await Task.Run(() => this.Void());
            await Task.Run(() => this.Void());

            Console.WriteLine($"Called AsyncVoid!!!!");
        }

        private async void Void()
        {
            for (int i = 0; i < 5; i++)
            {
                var cap = i;
                //await Task.Delay(1000);

                Console.WriteLine($"Call Void, count:{cap}");
            }
        }

        int _variable;
        AsyncLocal<string> _asyncLocal = new AsyncLocal<string>();
        ThreadLocal<string> _threadLocal = new ThreadLocal<string>();


        List<Task> taskList = new List<Task>();


        // DFS 알고리즘 ----------------------------------

        // https://jiwanm.github.io/algorithm%20lesson%202/chapter4-2/ 참고

        // 1. Graph : 행렬 형태의 그래프, 1 로 된 X좌표의 index는 연결된 노드 위치(Y좌표)를 뜻함.
        int[,] graph1 = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 1, 0 }
        };

        // 1.2 Graph : 그래프는 다른 선형구조처럼 클래스로 구현되어있지 않아 사용자가 편한 대로 구현하면 된다.
        List<int>[] graph2 = new List<int>[]{
                new List<int>(){1, 3},
                new List<int>(){0, 2, 3},
                new List<int>(){1},
                new List<int>(){0, 1, 4},
                new List<int>(){3,5},
                new List<int>(){4}
            };

        // 2. 탐색여부를 체크할 boolean array : 해당 array가 모두 true 값이 될 때, 종료조건이 된다.
        // graph의 size
        bool[] visited = new bool[6];

        // 3.1. graph1 탐색 DFS (재귀 사용)
        public void DFSByGraph1(int start)
        {
            Console.WriteLine(start);
            // 방문
            visited[start] = true;

            for (int next = 0; next < 6; next++)
            {
                // 연결되어 있지 않은 노드 스킵
                if (graph1[start, next] == 0)
                    continue;

                // 이미 방문한 노드 스킵
                if (visited[next])
                    continue;

                this.DFSByGraph1(next);
            }
        }

        // 3.2. graph2 탐색 DFS (재귀 사용)
        public void DFSByGraph2(int start)
        {
            Console.WriteLine(start);
            // 방문
            visited[start] = true;

            foreach (int next in graph2[start])
            {
                // 이미 방문한 노드 스킵
                if (visited[next])
                    continue;

                this.DFSByGraph2(next);
            }
        }

        // 그래프가 중간에 끊겨있을 경우를 대비해 모든 노드를 탐색하는 알고리즘
        public void SearchAll()
        {
            visited = new bool[6];
            for (int now = 0; now < 6; now++)
            {
                if (visited[now] == false)
                    DFSByGraph1(now);
            }
        }

        private async void Task1()
        {
            //System.Threading.Thread.Sleep(5000);
            await Task.Delay(1000);
            Console.WriteLine("Task1");
        }

        private async void Task2()
        {
            await Task.Delay(1000);
            Console.WriteLine("Task2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Task1();
            this.Task2();

            return;

            // 2번째 index의 나열된 노드부터 탐색 시작으로 연결된 모든 노드 탐색
            this.DFSByGraph1(2);
            this.DFSByGraph2(2);










            foreach (int idx in Enumerable.Range(0, 10))
            {
                try
                {
                    taskList.Add(Task.Run(() => { System.Threading.Thread.Sleep(1500); Console.WriteLine(idx); }));
                }
                catch(Exception ex)
                {
                    //
                }
            }

            Task.WaitAll(taskList.ToArray(), TimeSpan.FromSeconds(5));
            //taskList.RemoveAt(0);
            //taskList.RemoveAt(1);
            //taskList.RemoveAt(2);
            taskList.Clear();


            return;



            EntryAsyncVoid();
            //Async_f1();
            //Async_f2();

            Console.WriteLine("end!!!!!!!!!!!!!!!!!!!");


            return;




            //var tasks = Enumerable.Range(0, 10).Select(p => Output($"작업 {p}", p));
            //await Task.WhenAll(tasks);

            //return;

            //CallContext.LogicalSetData("key1", "Logical test");
            //CallContext.SetData("key11", "CallContext test");

            ////Task.Run(this.Logical_Test);
            //ThreadPool.QueueUserWorkItem(this.Logical_Test, null);

            //System.Threading.Thread.Sleep(5000);
            //var data1 = CallContext.LogicalGetData("key1");
            //var data2 = CallContext.GetData("key11");

            //var data3 = CallContext.LogicalGetData("key2");
            //var data4 = CallContext.GetData("key22");

            //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //Task.Factory.StartNew(() =>
            //{
            //    try
            //    {
            //        Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            //        this.button1.Text = "a";
            //    }
            //    catch (Exception ex)
            //    {
            //        //
            //    }
            //}, System.Threading.CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Current);

            //Task.Run(() =>
            //{
            //    try
            //    {
            //        this.button1.Text = "a";
            //    }  catch(Exception ex)
            //    {
            //        //
            //    }
            //});
        }

        private async Task Output(string name, int num)
        {
            await Task.Yield();

            _asyncLocal.Value = _asyncLocal.Value + " - " + num;
            _threadLocal.Value = _asyncLocal.Value + " - " + num;

            Console.WriteLine($"name : {name}");
            Console.WriteLine($"thread id : {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"asyncLocal : {_asyncLocal.Value}");
            Console.WriteLine($"threadLocal : {_threadLocal.Value}");

            Console.WriteLine("------------------------------");
        }

        private void Logical_Test(object objContext)
        {
            var data1 = CallContext.LogicalGetData("key1");
            var data2 = CallContext.GetData("key11");

            CallContext.LogicalSetData("key2", "Logical2 test");
            CallContext.SetData("key22", "CallContext2 test");
        }
    }
}
