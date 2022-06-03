using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public sealed class A
    {
        private static readonly object _objLock = new object();
        private static A _instance = null;
        //private static Lazy<A> _instance = new Lazy<A>(() => new A());

        public static A Instance
        {

            get
            {
                //lock (_objLock)
                //{
                    if (_instance == null)
                    {
                        _instance = new A();
                    }

                    return _instance;
                //}
            }
        }

        private A()
        {
            //Console.WriteLine($"ThreadId : {Thread.CurrentThread.ManagedThreadId}");
            //lock (_objLock)
            //{
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine(i);
            //}
            //}

            //lock (_objLock)
            //{
            Console.WriteLine(new Random().Next(100));
            //Console.WriteLine(System.IO.Path.GetRandomFileName());
            //}
        }


        public void DoSomething()
        {

        }
    }

    public class Ref
    {
        //public int _a = 1;

        public void Write()
        {
            int a = 2;
        }
    }

    public class QQQ
    {
        public string str = "a";
        public int num = 1;
    }

    public class 동물
    {
        //
    }

    public class 야옹이 : 동물
    {
        //
    }

    public class 멍멍이 : 동물
    {
        //
    }

    interface QQ<out T>
    {

    }

    class Foo
    {
        private List<int> _nums = new List<int>();
        private List<string> _strings = new List<string>();

        public List<int> GetNums()
        {
            _nums.AddRange(new int[] { 1, 2, 3 });
            return _nums;
        }

        public List<string> GetStrings()
        {
            _strings.AddRange(new string[] { "a", "b", "c" });
            return _strings;
        }

        public void Print()
        {
            _nums.ForEach(p => Console.WriteLine(p));
        }
    }

    class Program
    {
        

        private static async void Maina()
        {
            QQ<동물> 동물 = (QQ<멍멍이>)null;
            //QQ<야옹이> 야옹 = (QQ<동물>)null;



            Console.WriteLine("작업처리");
            await DoSomeAsync();
            Console.WriteLine("작업완료");
        }

        private static Task DoSomeAsync()
        {
            var task = Task.Run(() => { System.Threading.Thread.Sleep(1000 * 10); });
            return task;
        }

        private static void Print(int val)
        {
            Console.Write(val + " ");
        }

        static Action _printAction;


        abstract class PFoo
        {
            protected int _x;
            protected string _str;

            public PFoo(int x, string str)
            {
                _x = x;
                _str = str;
            }
            
            public abstract void Process();
            
            public void Print()
            {
                Console.WriteLine(_x);
                Console.WriteLine(_str);
            }
        }

        class CFoo : PFoo
        {
            public CFoo(int x, string str)
                : base(x, str)
            {
            }

            public override void Process()
            {
                int newX = _x;
                string newStr = _str;

                newX = 100;
                newStr = "world";
            }
        }

        public async void CallTask()
        {
            try
            {
                Thread.Sleep(5000);
                TestTask();
            }
            catch (Exception ex)
            {
                Console.WriteLine("error");
            }
        }

        public Task TestTask()
        {
            var task = Task.Run(() => {
                Console.WriteLine("task!");
                throw new Exception("!!!!!!");
            });

            return task;
        }

        

        static void Main(string[] args)
        {
            Foo foo = new Foo();
            var strings = foo.GetStrings();
            strings.Add("d");
            strings.Clear();
            strings = new List<string>();


            var nums = foo.GetNums();
            nums[0] = 5;
            //nums.Clear();

            foo.Print();


            int aa = 1;
            ref readonly int bb = ref aa;
            
            aa = 2;
            aa = 3;

            Console.WriteLine($"num1 : {aa}");
            Console.WriteLine($"num2 : {bb}");

            string a = "aaa";
            ref string b = ref a;
            a = "bbb";

            QQQ q = new QQQ();
            ref QQQ q2 = ref q;
            q.str = "b";
            q.num = 99;
            q = new QQQ();


            Program pp = new Program();
            pp.CallTask();
            Console.ReadLine();



            List<string> names = new List<string>();
            names.AddRange(new string[] { "Hess", "bluepope", "Foo" });

            var result = names.OrderBy(p => p).FirstOrDefault();

            CFoo c = new CFoo(10, "hello");
            c.Process();
            c.Print();

            for (int i = 0; i < 10; i++)
            {
                int ii = i;
                _printAction += () => Print(ii);
            }

            _printAction.Invoke();

            Maina();
            Console.WriteLine("aaaa");
            Console.WriteLine("bbbb");

            Ref refA = new Ref();
            refA.Write();
            Console.ReadLine();

            //Program pg = new Program();
            //pg.Click();
            //Console.ReadLine();






            //const int MAX = 1000;

            //var s2 = Stopwatch.StartNew();
            //// test string literal.
            //for (int i = 0; i < MAX; i++)
            //{
            //    if ("" == null)
            //    {
            //        throw new Exception();
            //    }
            //}

            //s2.Stop();

            //var s1 = Stopwatch.StartNew();
            //// test string.Empty.
            //for (int i = 0; i < MAX; i++)
            //{
            //    if (string.Empty == null)
            //    {
            //        throw new Exception();
            //    }
            //}

            //s1.Stop();


            //Console.WriteLine(((double)(s1.Elapsed.TotalMilliseconds * 1000000) /
            //    MAX).ToString("0.00 ns"));
            //Console.WriteLine(((double)(s2.Elapsed.TotalMilliseconds * 1000000) /
            //    MAX).ToString("0.00 ns"));

            //Console.ReadLine();





            //Thread[] array = new Thread[100];
            //for (int i = 0; i < array.Length; i++)
            //{
            //    array[i] = new Thread(() => A.Instance.DoSomething());
            //}



            //Parallel.ForEach(array, t => t.Start());

            ////foreach (var thread in array)
            ////{
            ////    thread.Start();
            ////}


            //Console.ReadLine();

























            // 01. json Load
            //string json = File.ReadAllText("data.json");
            //// 02. json Convert
            //dynamic jsonData = JsonConvert.DeserializeObject(json);

            //// 옵션 정보 리스트
            //List<Tuple<string, string>> optionList = new List<Tuple<string, string>>();

            //// 03. 파싱
            //for (int index = 0; index < jsonData.Count; index ++)
            //{
            //    string[] colorArr = null;
            //    string[] sizeArr = null;

            //    bool colorInfoToken = (index % 2 == 0);
            //    if(colorInfoToken)
            //    {
            //        // 03-01. 각 옵션 정보 파싱
            //        // 순서는 보장된다. ( {색상, 사이즈}, {색상, 사이즈} ... )
            //        colorArr = ParserColor(jsonData[index]);
            //        sizeArr = ParserSize(jsonData[index + 1]);

            //        // 03-02. 파싱된 옵션 조합
            //        CombinationOption(colorArr, sizeArr, out optionList);
            //    }
            //}

            //// 04. 출력
            //optionList.ForEach(item =>
            //{
            //    Console.WriteLine($"Color : {item.Item1} / Size : {item.Item2}");
            //});
        }

        private async void Click()
        {
            await DoSome();
            Console.WriteLine("bbbbbbb");
        }

        private Task DoSome()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine("aaaaaaaaaaaaa");
            });
        }

        static void CombinationOption(string[] colorArr, string[] sizeArr, out List<Tuple<string, string>> optionList)
        {
            optionList = new List<Tuple<string, string>>();

            if (colorArr.Length >= sizeArr.Length)
            {
                for (int c = 0; c < (colorArr.Length); c++)
                {
                    for (int s = 0; s < (sizeArr.Length); s++)
                    {
                        var opt = new Tuple<string, string>(colorArr[c], sizeArr[s]);
                        optionList.Add(opt);
                    }
                }
            }
            else
            {
                for (int s = 0; s < (sizeArr.Length); s++)
                {
                    for (int c = 0; c < (colorArr.Length); c++)
                    {
                        var opt = new Tuple<string, string>(colorArr[c], sizeArr[s]);
                        optionList.Add(opt);
                    }
                }
            }
        }

        static string[] ParserColor(dynamic colorInfoJson)
        {
            List<string> colorList = new List<string>();
            IEnumerable<JToken> propertyValues = colorInfoJson.SelectTokens("skuPropertyValues");

            foreach(JToken item in propertyValues)
            {
                var values = item.Select(p => p.SelectToken("skuPropertyValueTips").ToString()).Cast<string>();
                colorList.AddRange(values);
            }

            return colorList.ToArray();
        }

        static string[] ParserSize(dynamic sizeInfoJson)
        {
            List<string> sizeList = new List<string>();
            IEnumerable<JToken> propertyValues = sizeInfoJson.SelectTokens("skuPropertyValues");

            foreach (JToken item in propertyValues)
            {
                var values = item.Select(p => p.SelectToken("skuPropertyValueTips").ToString()).Cast<string>();
                sizeList.AddRange(values);
            }

            return sizeList.ToArray();
        }
    }
}
