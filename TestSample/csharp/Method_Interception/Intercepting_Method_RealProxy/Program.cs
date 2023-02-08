using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intercepting_Method_Cauldron.Interception
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var test = new TestClass();
            var res = test.Add(3, 4);
            Console.WriteLine(res);
            test.Val = 5;
            Console.WriteLine(test.Val);

            Console.ReadLine();
        }
    }


    [Logger]
    [OnPropertySet(nameof(ValueChangelog))]
    class TestClass
    {
        public int Add(int a, int b) => a + b;
        public int Val { get; set; }

        private void ValueChangelog(string propertyName, object newValue) =>
            Console.WriteLine($"Name : '{propertyName}' / Value : {newValue}");
    }
}
