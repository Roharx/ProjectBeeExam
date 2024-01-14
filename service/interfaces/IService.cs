namespace service.interfaces;

public interface IService
{
    IEnumerable<T> GetAllItems<T>(string tableName);
    int CreateItem<T>(string tableName, object createItemParameters);
    void UpdateItem<T>(string tableName, T item);
    void DeleteItem(string tableName, int itemId);
    IEnumerable<T> GetItemsByParameters<T>(string tableName, object parameters);
    T? GetSingleItemByParameters<T>(string tableName, object parameters);
}