using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Runtime.Remoting;
using System.Threading;
using System.Linq;

namespace ConsoleApp5
{
    public class Foo
    {
        BufferBlock<int> block1 = new BufferBlock<int>();
        BufferBlock<int> block2 = new BufferBlock<int>();

        Task block1Observer;
        Task block2Observer;

        public async Task Run()
        {
            block1Observer = Task.Run(async () => await Observe1());
            block2Observer = Task.Run(async () => await Observe2());

            await block1.SendAsync(11);
            await block1.SendAsync(12);
            await block1.SendAsync(13);

            //while (true)
            //{
            //    var read = Console.ReadLine();

            //    try
            //    {
            //        var block = read.Split(' ');

            //        var param1 = block[0];
            //        var param2 = block[1];

            //        int blockIndex = int.Parse(param1);
            //        int inputValue = int.Parse(param2);

            //        Task task = (blockIndex == 0) ? block1.SendAsync(inputValue)
            //                                      : block2.SendAsync(inputValue);
            //        await task;
            //    }
            //    catch (Exception)
            //    {
            //        Console.Clear();
            //        Console.WriteLine("Wrong Format!");
            //    }
            //}
        }

        private async Task Observe1()
        {
            try
            {
                while (true)
                {
                    int value = await block1.ReceiveAsync();
                    if (value == default)
                    {
                        Console.WriteLine("Observe1 End");
                        break;
                    }

                    PrintConsole(ConsoleColor.Yellow, $"Observer 1 Received: {value}");
                }
            }
            catch (Exception) { }
        }

        private async Task Observe2()
        {
            try
            {
                while (true)
                {
                    int value = await block2.ReceiveAsync();
                    if (value == default)
                    {
                        Console.WriteLine("Observe2 End");
                        break;
                    }

                    PrintConsole(ConsoleColor.Blue, $"Observer 2 Received: {value}");
                }
            }
            catch (Exception) { }
        }

        void PrintConsole(ConsoleColor color, string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
    }

    class Program
    {
        private static ThreadLocal<int> _tl = new ThreadLocal<int>();

        private static AsyncLocal<int> _al = new AsyncLocal<int>();

        private static async Task WorkAsync(int i)
        {
            if (_al.Value == 0)
            {
                _al.Value = i;
                Console.WriteLine($"[새로운 값] Thread Id : {Environment.CurrentManagedThreadId} - al Value : {_al.Value}");
            }
            else
            {
                Console.WriteLine($"[기존 값] Thread Id : {Environment.CurrentManagedThreadId} - al Value : {_al.Value}");
            }

            await Task.Delay(1000);

            Console.WriteLine($"[await 이후 값] Thread Id : {Environment.CurrentManagedThreadId} - al Value : {_al.Value}");

            Console.WriteLine("-----------------------------------------");
        }

        static async Task Main(string[] args)
        {
            ExecutionContext.SuppressFlow();
            // ThreadPool의 개수를 4개로 임의 제한
            ThreadPool.SetMinThreads(4, 4);
            ThreadPool.SetMaxThreads(4, 4);

            for (int i = 1; i < 11; i++)
            {
                await Task.Run(() => WorkAsync(i));
            }

            ExecutionContext.RestoreFlow();
            Console.Read();
            


            while (true)
            {
                Console.WriteLine("1부터 입력하는 수 의 합을 구합니다.");
                var input = Console.ReadLine();
                int numInput = 0;

                if (int.TryParse(input, out numInput) == false)
                {
                    Console.WriteLine("숫자만 입력해주세요!");
                    continue;
                }

                if (numInput <= 0)
                    break;

                string result = $"총 합은 {((numInput * (numInput + 1)) / 2)}입니다.";
                Console.WriteLine(result);

                Console.WriteLine("종료하시려면 숫자 0을 입력하세요!");
            }

            //var foo = new Foo();
            //await foo.Run();
        }
    }
}
