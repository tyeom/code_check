using EF_Test.Entity;
using EF_Test.Extensions;
using EF_Test.Repositories;
using EF_Test.ViewModel;

namespace EF_Test.Services
{
    public interface IRevenueService
    {
        Task<IEnumerable<RealizedPlEntity>> GetAllListAsync();

        Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Error(DateOnly createDate);

        /// <summary>
        /// (DateOnly)(object) 박싱 사용
        /// </summary>
        /// <param name="createDate"></param>
        /// <returns></returns>
        Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver1(DateTime createDate);

        Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver2(DateOnly createDate);

        Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver3(DateTime createDate);

        Task<RealizedPlEntity> CreateAsync(RealizedPlViewModel realizedPl);
    }

    public class RevenueService : IRevenueService
    {
        private readonly RevenueRepository _revenueRepository;

        public RevenueService(RevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        public async Task<IEnumerable<RealizedPlEntity>> GetAllListAsync()
        {
            return await _revenueRepository.GetAll();
        }

        public async Task<RealizedPlEntity> CreateAsync(RealizedPlViewModel realizedPl)
        {
            var entity = realizedPl.MapToEntity();
            await _revenueRepository.Add(entity);
            return entity;
        }

        public async Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Error(DateOnly createDate)
        {
            return await _revenueRepository.FindAll(p => DateOnly.ParseExact(p.CreateDate, "yyyy-MM-dd") >= createDate);
        }

        public async Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver1(DateTime createDate)
        {
            return await _revenueRepository.FindAll(p => (DateTime)(object)(p.CreateDate) >= createDate);
        }

        public async Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver2(DateOnly createDate)
        {
            //return await _revenueRepository.FindAll(p => p.CreateDate >= createDate);
            return null;
        }

        public async Task<IEnumerable<RealizedPlEntity>> GetAllListAsync_Cast_Ver3(DateTime createDate)
        {
            return await _revenueRepository.FindAll(p => p.CreateDate.ToDateTime(20) >= createDate);
        }
    }
}
