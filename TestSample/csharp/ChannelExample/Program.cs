using System;
using System.Collections.Concurrent;
using System.Threading.Channels;

//ConcurrentQueue 사용
//ConcurrentQueueEx ex1 = new();
//ex1.Start();

// Channel<T> 사용
// 채널에 배합(Back Pressure)조건 옵션 설정
var channelOptions = new BoundedChannelOptions(5)
{
    FullMode = BoundedChannelFullMode.Wait
};
// 항목 무한 추가 채널 생성
//var channel = Channel.CreateUnbounded<int>();
var channel = Channel.CreateBounded<int>(channelOptions);

// 외부로 제공하는 경우 이렇게 쓰기 전용과 읽기 전용 각 개별로 만들어서 제공하는 것이 좋다.
var producer = new Producer<int>(channel.Writer);
var consumer = new Consumer<int>(channel.Reader);

// 생산자
Task.Run(async () =>
{
    for (int i = 0; i < 10; i++)
    {
        //await producer.WriteAsync(i);
        bool result = producer.TryWrite(i);
        if (result == false)
        {
            Console.WriteLine("대기열 Full - 대기");
            await producer.WaitToWriteAsync();
        }
        else
        {
            Console.WriteLine($"{i} 추가");
        }
        await Task.Delay(1000);
    }
    // 채널 닫음
    //producer.Complete();
});

var reader = async () =>
{
    try
    {
        //int randomDelay = new Random().Next(1000, 3000);
        //Console.WriteLine($"randomDelay : {randomDelay}");

        while (true)
        {
            var item = await consumer.ReadAsync();
            Console.WriteLine($"> {item} / {Thread.CurrentThread.ManagedThreadId}");
            // 랜덤으로 1 ~ 3 초 대기
            //await Task.Delay(1000);
        }
    }
    catch (ChannelClosedException ex)
    {
        Console.WriteLine($"Channel was closed! / {Thread.CurrentThread.ManagedThreadId}");
    }
};

var reader2 = async () =>
{
    await Task.Delay(6000);

    await foreach (var item in consumer.ReadAllAsync())
    {
        Console.WriteLine($"소비 - {item}");
    }
};

// 소비자
reader2();

// 소비자 - 멀티 스레드
//Parallel.For(0, 2, (_) => reader2());

Console.ReadLine();
producer.TryWrite(100);
Console.ReadLine();


public class ConcurrentQueueEx
{
    ConcurrentQueue<int> _workingCollection = new();

    public async void Start()
    {
        Task.Run( async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                _workingCollection.Enqueue(i);
                await Task.Delay(1000);
            }
        });

        while (true)
        {
            int num;
            if (_workingCollection.TryDequeue(out num))
                Console.WriteLine($"> {num}");
        }
    }
}

/// <summary>
/// 생산 클래스 - Writer 전용
/// </summary>
/// <typeparam name="T"></typeparam>
public class Producer<T>
{
    private readonly ChannelWriter<T> _channelWriter;

    public Producer(ChannelWriter<T> channelWriter)
    {
        _channelWriter = channelWriter;
    }

    public bool TryWrite(T item)
    {
        return _channelWriter.TryWrite(item);
    }

    public ValueTask WriteAsync(T item)
    {
        return _channelWriter.WriteAsync(item);
    }

    public ValueTask<bool> WaitToWriteAsync()
    {
        return _channelWriter.WaitToWriteAsync();
    }

    public void Complete()
    {
        _channelWriter.Complete();
    }
}

/// <summary>
/// 소비 클래스 - Reader 전용
/// </summary>
/// <typeparam name="T"></typeparam>
public class Consumer<T>
{
    private readonly ChannelReader<T> _channelReader;

    public Consumer(ChannelReader<T> channelReader)
    {
        _channelReader = channelReader;
    }

    public ValueTask<T> ReadAsync()
    {
        return _channelReader.ReadAsync();
    }

    public IAsyncEnumerable<T> ReadAllAsync()
    {
        return _channelReader.ReadAllAsync();
    }
}