using infrastructure.Interfaces;
using service.interfaces;

namespace service
{
    public class ServiceBase : IService
    {
        protected readonly IRepository Repository;

        public ServiceBase(IRepository repository)
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

        public T GetSingleItemByParameters<T>(string tableName, object parameters)
        {
            return Repository.GetSingleItemByParameters<T>(tableName, parameters)
                   ?? throw new Exception($"Could not find item for the parameters in {tableName}.");
        }
        
        /// <summary>
        /// Retrieves data of type <typeparamref name="T"/> associated with a given identifier from a source table.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="sourceTableName">The name of the source table where the identifiers are located.</param>
        /// <param name="sourceId">The identifier object used to retrieve associated data. (eg.: new {account_id = id})</param>
        /// <param name="targetTableName">The name of the target table where the associated data is stored.</param>
        /// <returns>
        /// An IEnumerable of type <typeparamref name="T"/> containing the associated data.
        /// If no associated data is found, an empty IEnumerable is returned.
        /// </returns>
        public IEnumerable<T> GetDataForId<T>(string sourceTableName, object sourceId, string targetTableName)
        {
            var targetIds = GetItemsByParameters<int>(sourceTableName, sourceId).ToArray();

            return targetIds.Length != 0
                ? targetIds.Select(id => GetSingleItemByParameters<T>(targetTableName, new { id })).ToList()
                : Enumerable.Empty<T>();
        }

        public bool CreateItemWithoutReturn(string tableName, object createItemParameters)
        {
            return Repository.CreateItemWithoutReturn(tableName, createItemParameters);
        }

        public bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns)
        {
            return Repository.DeleteItemWithMultipleParams(tableName, conditionColumns);
        }
    }
}