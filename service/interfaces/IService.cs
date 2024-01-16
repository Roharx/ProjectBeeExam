namespace service.interfaces;

public interface IService
{
    IEnumerable<T> GetAllItems<T>(string tableName);
    int CreateItem<T>(string tableName, object createItemParameters);
    void UpdateItem<T>(string tableName, T item);
    void DeleteItem(string tableName, int itemId);
    IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters);
    T GetSingleItemByParameters<T>(string tableName, object parameters);
    
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
    IEnumerable<T> GetDataForId<T>(string sourceTableName, object sourceId, string targetTableName);
    bool CreateItemWithoutReturn(string tableName, object createItemParameters);
    bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns);
}