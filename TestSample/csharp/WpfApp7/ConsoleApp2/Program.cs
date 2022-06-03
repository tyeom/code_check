using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class qqqq
    {
        public string Name { get; set; }
    }

    public class bbbb
    {
        private readonly qqqq _q;
        public bbbb(qqqq q)
        {
            _q = q;
        }

        public bbbb()
        {
            
        }

        public void Proc()
        {
            if(_q != null)
                _q.Name = "aaaaaaa111";
        }
        
        ~bbbb()
        {
            //
        }
    }

    public class eeee
    {
        private readonly qqqq q = new qqqq();

        public void P()
        {
            bbbb b1 = new bbbb(q);
            b1.Proc();
            GC.Collect();

            bbbb b2 = new bbbb(q);
            b2.Proc();
            GC.Collect();

            bbbb b3 = new bbbb(q);
            b3.Proc();
            GC.Collect();

            bbbb b4 = new bbbb(q);
            b4.Proc();
            GC.Collect();

            bbbb b5 = new bbbb(q);
            b5.Proc();
            GC.Collect();
        }
    }









    class CustomItem
    {
        public int Idx { get; set; }
        public int intValue { get; set; }
        public string strValue { get; set; }
        public long longValue { get; set; }
    }

    class CustomItemIntEqualityComparer : IEqualityComparer<CustomItem>
    {
        public bool Equals(CustomItem x, CustomItem y)
        {
            return true;
            if (x.intValue != 0 && y.intValue != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //if (x.intValue == y.intValue)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public int GetHashCode([DisallowNull] CustomItem obj)
        {
            int hash = obj.intValue.GetHashCode();

            return hash;
        }
    }

    class CustomItemStringEqualityComparer : IEqualityComparer<CustomItem>
    {
        public bool Equals(CustomItem x, CustomItem y)
        {
            if (x.strValue.Equals(y.strValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode([DisallowNull] CustomItem obj)
        {
            int hash = obj.strValue.GetHashCode();

            return hash;
        }
    }

    class CustomItemLongEqualityComparer : IEqualityComparer<CustomItem>
    {
        public bool Equals(CustomItem x, CustomItem y)
        {
            if (x.longValue == y.longValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetHashCode([DisallowNull] CustomItem obj)
        {
            int hash = obj.longValue.GetHashCode();

            return hash;
        }
    }

    class ss
    {
        static ss instance = new ss();

        public static void Initialize()
        {
        }
    }

    class Program
    {
        //public static ss _ss = new ss();

        [STAThread]
        static void Main(string[] args)
        {
            string a = "a";
            Console.WriteLine(a!.Length);



            ss.Initialize();
        }

        static void AA()
        {
            while(true)
            {
                Thread.Sleep(3000);
                Console.WriteLine("1");
            }
        }

        static void BB()
        {
            while (true)
            {
                Thread.Sleep(3000);
                Console.WriteLine("2");
            }
        }
    }

    class TestClass
    {
        int Calc()
        {
            int result = 0;
            for (int i = 0; i < 50; ++i)
            {
                Console.WriteLine(i);
                result += i;
            }
            return result;
        }
        public async Task TestCall()
        {
            Console.WriteLine("Function TestCall Run");
            //Task<int> task = Task<int>.Factory.StartNew(() =>
            //{
            //    return Calc();
            //});
            //Console.WriteLine(task.Result.ToString());

            Console.WriteLine(await Task.FromResult<int>(Calc()));
            Console.WriteLine("Function TestCall End");
        }

        public async void Run()
        {
            await TestCall();
            Console.WriteLine("Run End");
        }
    }

}
