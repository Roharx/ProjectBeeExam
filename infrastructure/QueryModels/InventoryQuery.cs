namespace infrastructure.QueryModels;

public class InventoryQuery
{
    public int Id { get; set; }
    public int Field_Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}