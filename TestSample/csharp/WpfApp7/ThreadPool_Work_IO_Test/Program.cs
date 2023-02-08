namespace ThreadPool_Work_IO_Test;

internal class Program
{
    static void Main(string[] args)
    {
        ThreadPool.SetMinThreads(8, 1);  // I/O Thread를 1개로 제한 (두번째 인자가 I/O Thread 개수 제한)
        ThreadPool.SetMaxThreads(8, 1);  // I/O Thread를 1개로 제한 (두번째 인자가 I/O Thread 개수 제한)

        Console.ReadLine();

        //var workTH = new WorkTH();
        //workTH.Start();

        //var ioth = new IOTH();
        //ioth.Start();

        // I/O Thread로 동작됨을 체크
        var ioth = new IOTH_Check();
        ioth.Start();

        var ioth2 = new IOTH_Check();
        ioth2.Start();
        Console.ReadLine();
    }
}