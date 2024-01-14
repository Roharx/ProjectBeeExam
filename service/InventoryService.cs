using infrastructure.Interfaces;
using infrastructure.QueryModels;

namespace service;

public class InventoryService : ServiceBase
{
    public InventoryService(IRepository repository) : base (repository)
    { }

    public IEnumerable<InventoryQuery> GetAllInventoryItems()
    {
        return GetAllItems<InventoryQuery>("inventory");
    }

    public int CreateInventoryItem(int fieldId, string itemName, string itemDescription, int amount)
    {
        var parameters = new
        {
            field_id = fieldId,
            name = itemName,
            description = itemDescription,
            amount = amount
        };
        return CreateItem<int>("inventory", parameters);
    }

    public void UpdateInventoryItem(InventoryQuery item)
    {
        UpdateItem("inventory", item);
    }

    public void DeleteInventoryItem(int itemId)
    {
        DeleteItem("inventory", itemId);
    }

    public IEnumerable<InventoryQuery> GetItemsForField(int fieldId)
    {
        return GetItemsByParameters<InventoryQuery>("inventory", new { field_id = fieldId });
    }
}