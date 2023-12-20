namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateInventoryRequestDto
{
    public int Id { get; set; }
    public int FieldId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
}