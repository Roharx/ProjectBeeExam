namespace BeeProject.TransferModels.CreateRequests;

public class CreateBeeRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Comment { get; set; }
}