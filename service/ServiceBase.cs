using infrastructure.Interfaces;
using service.interfaces;

namespace service
{
    public class ServiceBase : IService
    {
        protected readonly IRepository Repository;

        protected ServiceBase(IRepository repository)
        {
            Repository = repository;
        }

        public IEnumerable<T> GetAllItems<T>(string tableName)
        {
            return Repository.GetAllItems<T>(tableName);
        }

        public int CreateItem<T>(string tableName, object createItemParameters)
        {
            var result = Repository.CreateItem<int>(tableName, createItemParameters);
            return result != -1 ? result : throw new Exception($"Couldn't create item in {tableName}.");
        }

        public void UpdateItem<T>(string tableName, T item)
        {
            var result = Repository.UpdateEntity(tableName, item, "id");
            if (!result)
                throw new Exception($"Couldn't update item in {tableName}.");
        }

        public void DeleteItem(string tableName, int itemId)
        {
            var result = Repository.DeleteItem(tableName, itemId);
            if (!result)
                throw new Exception($"Couldn't remove item from {tableName}.");
        }

        public IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters)
        {
            return Repository.GetItemsByParameters<T>(tableName, parameters);
        }

        public T? GetSingleItemByParameters<T>(string tableName, object parameters)
        {
            return Repository.GetSingleItemByParameters<T>(tableName, parameters)
                   ?? throw new Exception($"Could not find item for the parameters in {tableName}.");
        }
    }
}