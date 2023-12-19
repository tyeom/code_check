using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Principal;

namespace EF_Test.Base
{
    public abstract class EfCoreRepository<TEntity, TDBContext> : IRepository<TEntity>
        where TEntity : class, IEntity
        where TDBContext : DbContext
    {
        private readonly TDBContext _context;
        public EfCoreRepository(TDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TEntity?> Delete(long id)
        {
            var findEntity = await _context.Set<TEntity>().FindAsync(id);
            if (findEntity is null)
                return null;

            _context.Set<TEntity>().Remove(findEntity);
            await _context.SaveChangesAsync();

            return findEntity;
        }

        public async Task<TEntity> Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll(string includePropName)
        {
            return await _context.Set<TEntity>()
                .Include(includePropName)
                .ToListAsync();
        }

        public async Task<IEnumerable<ResultType>> GetGroupBy<ResultType>(Func<IQueryable<TEntity>, IQueryable<ResultType>> groupAndSelect)
            where ResultType : class
        {
            var entity = _context.Set<TEntity>();
            var grouped = groupAndSelect(entity);
            return await grouped.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>?> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            var findEntity = await _context.Set<TEntity>()
                .Where(predicate)
                .ToListAsync();
            return findEntity as IEnumerable<TEntity>;
        }

        public async Task<IEnumerable<TEntity>?> FindAll(Expression<Func<TEntity, bool>> predicate, string includePropName)
        {
            var findEntity = await _context.Set<TEntity>()
                .Include(includePropName)
                .Where(predicate).ToListAsync();
            return findEntity as IEnumerable<TEntity>;
        }

        public async Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate)
        {
            var findEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return findEntity;
        }

        public async Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, string includePropName)
        {
            var findEntity = await _context.Set<TEntity>()
                .Include(includePropName)
                .FirstOrDefaultAsync(predicate);
            return findEntity;
        }

        public async Task<TEntity?> FindFirst<TProperty>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TProperty>> includeExpression)
        {
            var findEntity = await _context.Set<TEntity>()
                .Include(includeExpression)
                .FirstOrDefaultAsync(predicate);
            return findEntity;
        }

        public async Task<TEntity?> FindFirst<TProperty>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TProperty>> includeExpression,
            Expression<Func<TProperty, TProperty>> thenIncludeExpression)
        {
            var findEntity = await _context.Set<TEntity>()
                .Include(includeExpression)
                .ThenInclude(thenIncludeExpression)
                .FirstOrDefaultAsync(predicate);
            return findEntity;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}