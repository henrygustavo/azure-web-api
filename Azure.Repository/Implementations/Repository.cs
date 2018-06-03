namespace Azure.Repository.Implementations
{
    using System.Collections.Generic;
    using Interfaces;
    using System.Threading.Tasks;
    using DocumentDB.Repository;

    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly DocumentDbRepository<T> _documentDbRepository;

        protected Repository(IDataBaseManager dataBaseManager)
        {
            IDocumentDbInitializer init = new DocumentDbInitializer();

            string endpoint = dataBaseManager.GetDataBaseEndPoint();
            string key = dataBaseManager.GetDataBaseKey();
            string databaseId = dataBaseManager.GetDataBaseId();

            var client = init.GetClient(endpoint, key);

            _documentDbRepository = new DocumentDbRepository<T>(client, databaseId);

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _documentDbRepository.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _documentDbRepository.GetByIdAsync(id);
        }

        public async Task<T> AddOrUpdateAsync(T obj)
        {
            return await _documentDbRepository.AddOrUpdateAsync(obj);
        }

        public async Task<bool> Removesync(string id)
        {
            return await _documentDbRepository.RemoveAsync(id);
        }
    }
}
