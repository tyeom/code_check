using System.Collections.Concurrent;

namespace ThreadPool_Work_IO_Test;

internal class WorkTH
{
    private BlockingCollection<String> _workingCollection = new();

    public void Start()
    {
        ThreadPool.QueueUserWorkItem(this.DoWork, null);

        while (true)
        {
            string? text = Console.ReadLine();
            if (string.IsNullOrEmpty(text))
                break;

            _workingCollection.TryAdd(text);
        }
    }

    private void DoWork(object? state)
    {
        while (true)
        {
            string? text;
            if (_workingCollection.TryTake(out text))
                Console.WriteLine($"> {text}");
        }
    }
}