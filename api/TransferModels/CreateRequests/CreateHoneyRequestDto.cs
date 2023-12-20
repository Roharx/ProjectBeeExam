namespace BeeProject.TransferModels.CreateRequests;

public class CreateHoneyRequestDto
{
    public string Name { get; set; }
    public bool Liquid { get; set; }
    public int Harvest { get; set; }
    public float Moisture { get; set; }
    public string Flowers { get; set; }
}