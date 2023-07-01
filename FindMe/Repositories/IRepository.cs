using FindMe.Models;

namespace FindMe.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetQueryableNoTracking();

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Update(T entity);

        Task DeleteAsync(int id);

        Task<bool> IsEntityExistAsync(int id);

        Task<IEnumerable<int>> GetNotExistEntitiesIdsAsync(IEnumerable<int> ids);

        void UpdateRange(IEnumerable<T> entities);

        void RemoveRange(IEnumerable<T> entities);
    }
}
