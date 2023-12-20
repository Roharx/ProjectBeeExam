namespace infrastructure.DataModels;

public class Field
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public InventoryItem[] Inventory { get; set; }
}