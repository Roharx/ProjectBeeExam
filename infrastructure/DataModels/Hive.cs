namespace infrastructure.DataModels;

public class Hive
{
    public int Id { get; set; }
    public int FieldId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string PlacementDate { get; set; }
    public string LastCheck { get; set; }
    public bool ReadyToHarvest { get; set; }
    public Ailment[] Ailments { get; set; }
    public string Color { get; set; }
    public Task[] Tasks { get; set; }
    public Bee Bees { get; set; }
    public Harvest[] Harvests { get; set; }
    public string? Comment { get; set; }
}