namespace BeeProject.TransferModels.UpdateRequests;

public class UpdateHoneyRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Liquid { get; set; }
    public int Harvest { get; set; }
    public float Moisture { get; set; }
    public string Flowers { get; set; }
}