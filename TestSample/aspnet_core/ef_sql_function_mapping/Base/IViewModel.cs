using EF_Test.Entity;

namespace EF_Test.Base
{
    public interface IViewModel<TEntity>
        where TEntity : IEntity
    {
        public TEntity MapToEntity();
    }
}
