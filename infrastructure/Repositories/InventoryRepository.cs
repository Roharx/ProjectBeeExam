using infrastructure.QueryModels;
using Npgsql;

namespace infrastructure.Repositories;

public class InventoryRepository : RepositoryBase
{
    private readonly NpgsqlDataSource _dataSource;

    public InventoryRepository(NpgsqlDataSource dataSource) : base(dataSource)
    {
        _dataSource = dataSource;
    }

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

    public bool UpdateInventory(InventoryQuery inventory)
    {
        return UpdateEntity("inventory", inventory, "id");
    }

    public bool DeleteInventory(int inventoryId)
    {
        return DeleteItem("inventory", inventoryId);
    }

    public IEnumerable<InventoryQuery> GetInventoryForField(int fieldId)
    {
        return GetItemsByParameters<InventoryQuery>("inventory", new { field_id = fieldId });
    }
}