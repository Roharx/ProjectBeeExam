namespace infrastructure.QueryModels;

public class HoneyQuery
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Liquid { get; set; }
    public int Harvest_id { get; set; }
    public float Moisture { get; set; }
    public string Flowers { get; set; }
}