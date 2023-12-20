namespace BeeProject.TransferModels.CreateRequests;

public class CreateInventoryRequestDto
{
    public int FieldId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}