namespace SimpleMVVMWpf.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Models;

    /// <summary>
    /// 서버에서 데이터를 받아오는 부분이라고 가정
    /// </summary>
    public interface IDBService
    {
        Task<IEnumerable<T>> GetData<T>(int limitCount) where T : new();
        Task<bool> SaveData<T>(T dataModel);
    }

    public class DBService : IDBService
    {
        public DBService()
        {
            //
        }

        public async Task<IEnumerable<T>> GetData<T>(int limitCount) where T : new()
        {
            // 서버에서 데이터를 받아온다고 가정

            List<T> sampleModelList = new List<T>();
            var task = Task.Run(() =>
            {
                for (int i = 0; i < limitCount; i++)
                {
                    T instance = new T();
                    sampleModelList.Add(instance);
                }
                return sampleModelList.AsEnumerable();
            });
            return await task;
        }

        public async Task<bool> SaveData<T>(T dataModel)
        {
            var task = Task.Run(() => { return true; });
            return await task;
        }
    }
}
