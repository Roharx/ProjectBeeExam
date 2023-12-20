namespace infrastructure.DataModels;

public class Honey
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Liquid { get; set; }
    public int HarvestId { get; set; }
    public float Moisture { get; set; }
    public string Flowers { get; set; }
}