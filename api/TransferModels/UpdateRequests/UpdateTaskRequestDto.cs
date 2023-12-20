namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateTaskRequestDto
{
    public int Id { get; set; }
    public int HiveId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool Done { get; set; }
}