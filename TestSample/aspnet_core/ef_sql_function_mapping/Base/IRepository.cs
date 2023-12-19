using System.Linq.Expressions;

namespace EF_Test.Base
{

    public interface IRepository<T> where T : class, IEntity
    {
        /// <summary>
        /// 페이징 없이 모든 데이터 조회
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll(string includePropName);

        /// <summary>
        /// 데이터 조회 - where
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>?> FindAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 데이터 추가
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> Add(T entity);

        /// <summary>
        /// 데이터 수정
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Update(T entity);

        /// <summary>
        /// 고유 Key로 데이터 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> Delete(long id);

        /// <summary>
        /// 데이터 삭제
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> Delete(T entity);
    }
}