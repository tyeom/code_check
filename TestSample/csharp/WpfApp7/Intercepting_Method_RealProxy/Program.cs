using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intercepting_Method_RealProxy
{
    internal class Program
    {
        private int Add(int a, int b) => a + b;
        public int Val { get; set; }

        static void Main(string[] args)
        {
            var test = new TestClass();
            var proxy = RuntimeProxy.Create<TestClass>(test,
                // 실제 클래스 메서드 호출 전 가로채기 처리 Func
                t =>
                {
                    try
                    {
                        Console.WriteLine("Executing...!");
                        return t.Invoke();
                    }
                    finally
                    {
                        Console.WriteLine("Executed!");
                    }
                });

            var res = proxy.Add(3, 4);
            Console.WriteLine(res);
            proxy.Val= 5;
            Console.WriteLine(proxy.Val);

            Console.ReadLine();
        }
    }

    class TestClass : MarshalByRefObject
    {
        public int Add(int a, int b) => a + b;
        public int Val { get; set; }
    }
}
