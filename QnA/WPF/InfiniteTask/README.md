#### 질문내용

https://forum.dotnetdev.kr/t/infinite-task/4621

***

**Task에 infinite loop가 들어가는데 이 값을 binding 하여 UI에 값을 업데이트 하려고 한다면 어떻게 해야 할까요?**

제가 짜본 코드 입니다.

```cs
async void FooAsync()
{
    CancellationTokenSource source = new CancellationTokenSource();
    var token = source.Token;
    var result = await Task.Run(() => DoWork(token), token);

    token.ThrowIfCancellationRequested(); // <-여기서 부터 실행되지 않음

    foreach (var item in result)
    {
        Console.WriteLine(item.ToString());
    }
}

IEnumerable<TResult> DoWork(CancellationToken token)
{
    while(!token.IsCancellationRequested) 
    { 
        /* Do something */
       yield return result; 
    }
}
```
여기서 문제는 FooAsync 의 result가 문제인데요. Task가 infinite다 보니 result에서 무한 대기상태에 걸립니다.

어디가 잘못 된 것일까요? 접근방법이 문제가 될 수도 있다는 생각이 듭니다.

***
 
#### 결과
![33](https://user-images.githubusercontent.com/13028129/191411928-d057d047-2b03-49d8-b17a-e23181a22c59.gif)
