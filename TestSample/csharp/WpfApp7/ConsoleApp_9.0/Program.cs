using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp_9._0;

public record Point
{
    public int x;
    public int y;
}

public struct aa
{
    public int x;

    public override string ToString()
    {
        return base.ToString();
    }
}

public interface ILogger
{
    public void Foo(string xx)
    {
        Console.WriteLine(xx);
    }
}

public class Logger
{
    public void Foo(string xx)
    {
        Console.WriteLine(xx);
    }
}

public class Account
{
    private string _name;
    public Account(string name)
    {
        _name = name;

        if (name == null || name == "해커")
            throw new Exception("계좌 접근 금지!!");
    }

    public void 출금(int amount)
    {
        Console.WriteLine($"{amount} 출금됨!");
    }
}

public class AttackAccount : Account
{
    public AttackAccount(string name) :
        base(name)
    { }

    ~AttackAccount()
    {
        base.출금(100000);
    }
}

public class AForm
{
    private BForm _b;
    public AForm(BForm b)
    {
        _b = b;
        _b = null;
    }

    private void QQQ()
    {

    }
}

public class BForm
{
    public void CCC()
    {

    }
}

public class Program : Logger
{
    public static async void RequestAsync()
    {
        //Task.Run(async () =>
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("1111111");
        //        //Thread.Sleep(5000);
        //        await Task.Delay(5000);
        //    }
        //});
        while (true)
        {
            Console.WriteLine("1111111");
            //Thread.Sleep(5000);
            await Task.Delay(5000);
        }
    }

    static void Main(string[] args)
    {
        //BForm b = new BForm();
        //AForm a = new AForm(b);
        //b.CCC();

        Thread thread = null;
        if (thread == null)
        {
            thread = new Thread(new ThreadStart(RequestAsync));
            thread.Start();
        }
        else if (thread.IsAlive)
        {
            thread.Interrupt();
        }
        else
        {
            thread.Start();
        }

        Console.ReadLine();

        if (thread == null)
        {
            thread = new Thread(new ThreadStart(RequestAsync));
            thread.Start();
        }
        else if (thread.IsAlive)
        {
            thread.Interrupt();
        }
        else
        {
            thread.Start();
        }




        //AttackAccount attack = new AttackAccount("해커");
        //attack.출금(100000);

        //AttackAccount attack = null;
        //try
        //{
        //    for (int j = 0; j < 100; j++)
        //    {
        //        attack = new AttackAccount("해커");
        //    }

        //    //attack = new AttackAccount("해커");
        //}  catch (Exception e)
        //{
        //    //
        //}

        //attack = null;
        //GC.Collect();
        //GC.WaitForPendingFinalizers();

        ////new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((p) => { while (true) { } })).Start();


        //Program p = new Program();
        //p.Foo("aaa");

        //System.Drawing.Color c1 = System.Drawing.Color.FromArgb(0, 1, 254);
        //System.Drawing.Color c2 = System.Drawing.Color.FromArgb(0, 1, 254);

        //if (c1 == c2)
        //{

        //}

        //if (c1.Equals(c2))
        //{

        //}
    }
}