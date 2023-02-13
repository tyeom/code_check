namespace Rate_limiting_Example.Services
{
    public interface IGetTimeService
    {
        /// <summary>
        /// 현재 시간 반환
        /// </summary>
        /// <returns></returns>
        TimeOnly currentTime();
    }

    public class GetTimeService : IGetTimeService
    {
        public GetTimeService()
        {
        }

        public TimeOnly currentTime()
        {
            // TimeOnly 구조체 : 하루 시간 단위를 나타내는 구조체
            return TimeOnly.FromDateTime(DateTime.Now);
        }
    }
}
