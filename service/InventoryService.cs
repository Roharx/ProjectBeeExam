using infrastructure.Interfaces;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class InventoryService
{
    private readonly IRepository _repository;
    public InventoryService(IRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<InventoryQuery> GetAllInventoryItems()
    {
        return _repository.GetAllItems<InventoryQuery>("inventory");
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
        var result = _repository.CreateItem<int>("inventory", parameters);
        return result != -1 ? result : throw new Exception("Could not create item");
    }

    public void UpdateInventoryItem(InventoryQuery item)
    {
        if (!_repository.UpdateEntity("inventory", item, "id"))
            throw new Exception("Could not update item.");
    }

    public void DeleteInventoryItem(int itemId)
    {
        if (!_repository.DeleteItem("inventory", itemId))
            throw new Exception("Could not remove item.");
    }

    public IEnumerable<InventoryQuery> GetItemsForField(int fieldId)
    {
        return _repository.GetItemsByParameters<InventoryQuery>("inventory", new { field_id = fieldId });
    }
}