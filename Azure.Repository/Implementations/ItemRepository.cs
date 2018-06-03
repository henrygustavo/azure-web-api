namespace Azure.Repository.Implementations
{
    using Entity;
    using Interfaces;

    public class ItemRepository: Repository<Item>, IItemRepository
    {
        public ItemRepository(IDataBaseManager dataBaseManager) : base(dataBaseManager)
        {
        }
    }
}
