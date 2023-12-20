using System.Runtime.InteropServices.JavaScript;
using infrastructure.QueryModels;
using infrastructure.Repositories;

namespace service;

public class InventoryService
{
    private readonly InventoryRepository _inventoryRepository;

    public InventoryService(InventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public IEnumerable<InventoryQuery> GetAllInventoryItems()
    {
        return _inventoryRepository.GetAllInventoryItems();
    }

    public int CreateInventoryItem(int fieldId, string itemName, string itemDescription, int amount)
    {
        var result = _inventoryRepository.CreateInventoryItem(fieldId, itemName, itemDescription, amount);
        return result != -1 ? result : throw new Exception("Could not create item");
    }

    public void UpdateInventoryItem(InventoryQuery item)
    {
        if (!_inventoryRepository.UpdateInventory(item))
            throw new Exception("Could not update item.");
    }

    public void DeleteInventoryItem(int itemId)
    {
        if (!_inventoryRepository.DeleteInventory(itemId))
            throw new Exception("Could not remove item.");
    }

    public IEnumerable<InventoryQuery> GetItemsForField(int fieldId)
    {
        return _inventoryRepository.GetInventoryForField(fieldId);
    }
}