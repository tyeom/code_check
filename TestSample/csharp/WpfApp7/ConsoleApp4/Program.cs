using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
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

            while (true)
            {
                var read = Console.ReadLine();

                try
                {
                    var block = read.Split(' ');

                    var param1 = block[0];
                    var param2 = block[1];

                    int blockIndex = int.Parse(param1);
                    int inputValue = int.Parse(param2);

                    Task task = (blockIndex == 0) ? block1.SendAsync(inputValue)
                                                  : block2.SendAsync(inputValue);
                    await task;
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Wrong Format!");
                }
            }
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
        static async void Main(string[] args)
        {
            var foo = new Foo();
            await foo.Run();
        }
    }
}
