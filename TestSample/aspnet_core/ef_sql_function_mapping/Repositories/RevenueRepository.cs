using EF_Test.Base;
using EF_Test.DataContext;
using EF_Test.Entity;

namespace EF_Test.Repositories
{
    public class RevenueRepository : EfCoreRepository<RealizedPlEntity, EFTest_DBContext>
    {
        public RevenueRepository(EFTest_DBContext context)
        : base(context)
        {
        }
    }
}
