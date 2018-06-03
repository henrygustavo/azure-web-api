namespace Azure.Repository.Interfaces
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> AddOrUpdateAsync(T obj);
        Task<bool> Removesync(string id);
    }
}
