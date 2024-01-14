namespace infrastructure.Interfaces;

public interface IRepository
{
    IEnumerable<T> GetAllItems<T>(string tableName);
        
    IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters);
        
    T? GetSingleItemByParameters<T>(string tableName, object parameters);
        
    bool DeleteItem(string tableName, int itemId);
        
    int CreateItem<T>(string tableName, object parameters);
        
    bool UpdateEntity<T>(string tableName, T entity, string conditionColumnName);
    
    bool ModifyItem(string tableName, Dictionary<string, object> conditionColumns, Dictionary<string, object> modifications);
    
    IEnumerable<T> GetSelectedParametersForItems<T>(string tableName, string columns, object parameters);

    bool DeleteItemWithMultipleParams(string tableName, Dictionary<string, object> conditionColumns);
    bool CreateItemWithoutReturn(string tableName, object parameters);
}