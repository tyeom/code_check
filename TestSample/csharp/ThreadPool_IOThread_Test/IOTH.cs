using System.Collections.Concurrent;

namespace ThreadPool_Work_IO_Test;

internal class IOTH
{
    private AutoResetEvent _are = new(false);
    private BlockingCollection<String> _workingCollection = new();

    public void Start()
    {
        var regWaitHandle =
            ThreadPool.RegisterWaitForSingleObject(_are, this.DoWork, null, -1, false);

        while (true)
        {
            string? text = Console.ReadLine();
            if (string.IsNullOrEmpty(text))
                break;

            _workingCollection.TryAdd(text);
            _are.Set();
        }

        regWaitHandle.Unregister(_are);
    }

    private void DoWork(object? state, bool timeOut)
    {
        string? text;
        if (_workingCollection.TryTake(out text))
            Console.WriteLine($"> {text}");
    }
}

internal class IOTH_Check
{
    private AutoResetEvent _are = new(false);
    private BlockingCollection<String> _workingCollection = new();

    public void Start()
    {
        var regWaitHandle =
            ThreadPool.RegisterWaitForSingleObject(_are, this.DoWork, null, -1, false);

        _workingCollection.TryAdd("1");
        _are.Set();

        regWaitHandle.Unregister(_are);
    }

    private void DoWork(object? state, bool timeOut)
    {
        string? text;
        if (_workingCollection.TryTake(out text))
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {DateTime.Now} > {text}");
            Thread.Sleep(5000);
        }
    }
}